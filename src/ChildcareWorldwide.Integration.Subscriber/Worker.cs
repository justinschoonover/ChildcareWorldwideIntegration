using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Google.Api;
using ChildcareWorldwide.Google.Api.Models;
using ChildcareWorldwide.Google.Api.PubSub;
using ChildcareWorldwide.Hubspot.Api;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Integration.Subscriber.Helpers;
using ChildcareWorldwide.Integration.Subscriber.Mappers;
using ChildcareWorldwide.Integration.Subscriber.Models;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog;

namespace ChildcareWorldwide.Integration.Subscriber
{
	public class Worker : BackgroundService
	{
		private const int DefaultCacheTimeInMinutes = 30;
		private const string OptOutCacheKey = "EMAIL_OPT_OUT";

		// limit number of messages that are being processed by subscribers
		private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(10);
		private readonly TimeSpan m_semaphoreWaitTimespan = TimeSpan.FromSeconds(10);

		private readonly Logger m_logger;
		private readonly IDrapiService m_drapiService;
		private readonly IHubspotService m_hubspotService;
		private readonly IMemoryCache m_cache;
		private readonly IGoogleCloudPubSubService m_googleCloudPubSubService;
		private readonly IGoogleCloudFirestoreService m_googleCloudFirestoreService;

		public Worker(
			IDrapiService drapiService,
			IHubspotService hubspotService,
			IGoogleCloudPubSubService googleCloudPubSubService,
			IGoogleCloudFirestoreService googleCloudFirestoreService,
			IMemoryCache memoryCache)
		{
			m_logger = LogManager.GetCurrentClassLogger();
			m_cache = memoryCache;

			m_drapiService = drapiService;
			m_hubspotService = hubspotService;
			m_googleCloudPubSubService = googleCloudPubSubService;
			m_googleCloudFirestoreService = googleCloudFirestoreService;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			// ensure topics and subscriptions exist
			await m_googleCloudPubSubService.InitalizeAsync(cancellationToken);

			// start subscribers
			await StartSubscriberWithErrorHandlingAsync(Subscriptions.HubspotGetDonorsForImport, QueueDonorsForImportHandler());
			await StartSubscriberWithErrorHandlingAsync(Subscriptions.HubspotImportDonorAsCompany, ImportCompanyFromDonorHandler());
			await StartSubscriberWithErrorHandlingAsync(Subscriptions.HubspotImportDonorAsContact, ImportContactFromDonorHandler());

			// do the thing!
			while (!cancellationToken.IsCancellationRequested)
				await Task.Delay(1000, cancellationToken);

			foreach (string? topicId in Topics.AllTopics)
				await m_googleCloudPubSubService.ShutdownPublisherAsync(topicId, cancellationToken);

			foreach (var (_, subscriptionId) in Subscriptions.AllSubscriptions)
				await m_googleCloudPubSubService.StopSubscriberAsync(subscriptionId, cancellationToken);
		}

		private Task StartSubscriberWithErrorHandlingAsync((string Topic, string Subscription) config, Func<PubsubMessage, CancellationToken, Task<ProcessMessageResult>> handler) =>
			m_googleCloudPubSubService.StartSubscriberAsync(
				config.Subscription,
				async (msg, cancellationToken) =>
				{
					var result = await handler(msg, cancellationToken);
					if (result.Exception != null)
					{
						var messagingError = new MessagingError
						{
							Topic = config.Topic,
							MessageData = msg.Data,
							DenariAccount = result.Donor?.Account,
							Email = result.Email,
							Error = result.Exception.Message,
						};
						await m_googleCloudFirestoreService.AddMessagingErrorAsync(messagingError, cancellationToken);
					}

					return result.Response;
				});

		private Func<PubsubMessage, CancellationToken, Task<ProcessMessageResult>> QueueDonorsForImportHandler() =>
			async (msg, cancellationToken) =>
			{
				// limit concurrency without holding messages for unreasonably long times
				if (await IsConcurrencyLimitReached(cancellationToken))
					return new ProcessMessageResult(SubscriberClient.Reply.Nack);

				// make sure email blacklist cache is hydrated
				await IsEmailOptedOutAsync(string.Empty);

				var publishTasks = new List<Task<string>>();
				try
				{
					if (msg.Data.ToStringUtf8().Equals("all", StringComparison.InvariantCultureIgnoreCase))
					{
						m_logger.Info("Getting all Donors from Denari...");
						await foreach (var donor in m_drapiService.GetDonorsAsync(cancellationToken))
						{
							await QueueImportTask(donor, publishTasks);

							// TODO: For testing, remove
							if (publishTasks.Count > 1000)
								break;
						}
					}
					else
					{
						var (donor, _) = await m_drapiService.GetDonorByAccountAsync(msg.Data.ToStringUtf8(), cancellationToken);
						if (donor != null)
							await QueueImportTask(donor, publishTasks);
					}

					if (cancellationToken.IsCancellationRequested)
					{
						m_semaphore.Release();
						return new ProcessMessageResult(SubscriberClient.Reply.Nack);
					}

					await Task.WhenAll(publishTasks);
					m_logger.Info("Finished getting all Donors from Denari for import.");

					m_semaphore.Release();
					return new ProcessMessageResult(SubscriberClient.Reply.Ack);
				}
				catch (Exception e)
				{
					m_logger.Error(e);
					m_semaphore.Release();
					return new ProcessMessageResult(SubscriberClient.Reply.Ack)
					{
						Exception = e,
					};
				}

				async Task QueueImportTask(Donor donor, ICollection<Task<string>> tasks)
				{
					m_logger.Debug($"Getting Denari classifications for donor #{donor.Account}");
					var classifications = await m_drapiService.GetClassificationsForDonorAsync(donor.DonorKey, cancellationToken).ToListAsync(cancellationToken);

					m_logger.Debug($"Publishing import event for donor #{donor.Account}");
					string json = JsonConvert.SerializeObject(donor with { Classifications = classifications });
					tasks.Add(m_googleCloudPubSubService.PublishMessageAsync(Topics.HubspotImportFromDonor, json));
				}
			};

		private Func<PubsubMessage, CancellationToken, Task<ProcessMessageResult>> ImportCompanyFromDonorHandler() => async (msg, cancellationToken) =>
		{
			// limit concurrency without holding messages for unreasonably long times
			if (await IsConcurrencyLimitReached(cancellationToken))
				return new ProcessMessageResult(SubscriberClient.Reply.Nack);

			var donor = JsonConvert.DeserializeObject<Donor>(msg.Data.ToStringUtf8());

			if ((donor.Email.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email))
				|| (donor.Email2.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email2)))
			{
				m_logger.Debug($"Importing company from donor #{donor.Account}");
				try
				{
					var company = await m_hubspotService.CreateOrUpdateCompanyAsync(IntegrationMapper.MapDonorToCompany(donor), cancellationToken);
					await m_googleCloudFirestoreService.AddDenariCompanyAssociationAsync(donor.Account, company.Id, cancellationToken);
					await TrueUpHubspotAssociations(donor, cancellationToken);
				}
				catch (Exception e)
				{
					m_logger.Error(e, $"Error creating or updating company in Hubspot for donor {donor.Account}.");
					m_semaphore.Release();
					return new ProcessMessageResult(SubscriberClient.Reply.Ack)
					{
						Donor = donor,
						Exception = e,
					};
				}
			}

			m_semaphore.Release();
			return new ProcessMessageResult(SubscriberClient.Reply.Ack);
		};

		private Func<PubsubMessage, CancellationToken, Task<ProcessMessageResult>> ImportContactFromDonorHandler() => async (msg, cancellationToken) =>
		{
			// limit concurrency without holding messages for unreasonably long times
			if (await IsConcurrencyLimitReached(cancellationToken))
				return new ProcessMessageResult(SubscriberClient.Reply.Nack);

			var donor = JsonConvert.DeserializeObject<Donor>(msg.Data.ToStringUtf8());
			ProcessMessageResult? result = null;

			var contactIds = new List<string>();

			var contact = await ImportContactForEmail(donor.Email);
			if (contact != null)
				contactIds.Add(contact.Id);

			if (donor.Email2 != donor.Email)
			{
				contact = await ImportContactForEmail(donor.Email2);
				if (contact != null)
					contactIds.Add(contact.Id);
			}

			await m_googleCloudFirestoreService.AddDenariContactAssociationsAsync(donor.Account, contactIds, cancellationToken);
			await TrueUpHubspotAssociations(donor, cancellationToken);

			m_semaphore.Release();
			return result ?? new ProcessMessageResult(SubscriberClient.Reply.Ack);

			async Task<Contact?> ImportContactForEmail(string? email)
			{
				if (!email.IsValidEmailAddress() || await IsEmailOptedOutAsync(email))
					return null;

				try
				{
					m_logger.Debug($"Importing contact from donor #{donor.Account} for email {email}");
					return await m_hubspotService.CreateOrUpdateContactAsync(IntegrationMapper.MapDonorToContact(donor, email), cancellationToken);
				}
				catch (Exception e)
				{
					m_logger.Error(e, $"Error creating or updating contact in Hubspot for email {email}.");
					result = new ProcessMessageResult(SubscriberClient.Reply.Ack)
					{
						Donor = donor,
						Email = email,
						Exception = e,
					};
				}

				return null;
			}
		};

		private async Task<bool> IsConcurrencyLimitReached(CancellationToken cancellationToken)
		{
			using var reasonableWaitCancellationTokenSource = new CancellationTokenSource(m_semaphoreWaitTimespan);
			using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(reasonableWaitCancellationTokenSource.Token, cancellationToken);
			try
			{
				await m_semaphore.WaitAsync(linkedCts.Token);
			}
			catch (OperationCanceledException)
			{
				return true;
			}

			return false;
		}

		private async Task TrueUpHubspotAssociations(Donor donor, CancellationToken cancellationToken)
		{
			var association = await m_googleCloudFirestoreService.GetHubspotAssociationsForDonorAsync(donor.Account, cancellationToken);
			if (association?.CompanyId == null)
				return;
			if (association.ContactIds == null)
				return;

			var company = new Company
			{
				Id = association.CompanyId!,
			};

			foreach (string? contactId in association.ContactIds)
			{
				await m_hubspotService.AssociateCompanyAndContactAsync(
					company,
					new Contact
					{
						Id = contactId!,
					},
					cancellationToken);
			}
		}

		private async Task<bool> IsEmailOptedOutAsync(string? email)
		{
			if (email == null)
				return true;

			var optedOutList = await m_cache.GetOrCreateAsync(
				OptOutCacheKey,
				async entry =>
				{
					entry.SetSlidingExpiration(TimeSpan.FromMinutes(DefaultCacheTimeInMinutes));
					return await m_hubspotService.GetOptedOutEmailsAsync();
				});

			return optedOutList.Contains(email);
		}
	}
}
