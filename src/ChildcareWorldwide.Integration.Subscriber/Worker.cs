using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Google.Api;
using ChildcareWorldwide.Google.Api.PubSub;
using ChildcareWorldwide.Hubspot.Api;
using ChildcareWorldwide.Integration.Subscriber.Helpers;
using ChildcareWorldwide.Integration.Subscriber.Mappers;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog;

namespace ChildcareWorldwide.Integration.Subscriber
{
    public class Worker : BackgroundService
    {
        private const string OptOutCacheKey = "EMAIL_OPT_OUT";

        private readonly Logger m_logger;
        private readonly IDrapiService m_drapiService;
        private readonly IHubspotService m_hubspotService;
        private readonly IMemoryCache m_cache;
        private readonly IGoogleCloudPubSubService m_googleCloudPubSubService;

        public Worker(IDrapiService drapiService, IHubspotService hubspotService, IGoogleCloudPubSubService googleCloudPubSubService, IMemoryCache memoryCache)
        {
            m_logger = LogManager.GetCurrentClassLogger();
            m_cache = memoryCache;

            m_drapiService = drapiService;
            m_hubspotService = hubspotService;
            m_googleCloudPubSubService = googleCloudPubSubService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // ensure topics and subscriptions exist
            await m_googleCloudPubSubService.InitalizeAsync(cancellationToken);

            // cache email blacklist
            m_logger.Info("Caching email blacklist for 120 minutes");
            m_cache.Set(OptOutCacheKey, await m_hubspotService.GetOptedOutEmailsAsync(), TimeSpan.FromMinutes(120));

            // start subscribers
            await m_googleCloudPubSubService.StartSubscriberAsync(Subscriptions.HubspotGetDonorsForImport.Subscription, QueueDonorsForImportHandler());
            await m_googleCloudPubSubService.StartSubscriberAsync(Subscriptions.HubspotImportDonorAsCompany.Subscription, ImportCompanyFromDonorHandler());
            await m_googleCloudPubSubService.StartSubscriberAsync(Subscriptions.HubspotImportDonorAsContact.Subscription, ImportContactFromDonorHandler());

            // do the thing!
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }

            foreach (var topicId in Topics.AllTopics)
                await m_googleCloudPubSubService.ShutdownPublisherAsync(topicId, cancellationToken);

            foreach (var (_, subscriptionId) in Subscriptions.AllSubscriptions)
                await m_googleCloudPubSubService.StopSubscriberAsync(subscriptionId, cancellationToken);
        }

        private Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> ProcessQueueWithErrorHandlingAsync(Func<Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>>> handler)
        {
            const int maxTries = 3;
            int tryCount = 0;

            try
            {
                return handler();
            }
            catch (Exception e)
            {
                tryCount++;

                // retry if we've not exceeded the limit or it's likely this is a recoverable issue
                if (tryCount < maxTries && IsLikelyTemporaryFault(e))
                    return (msg, cancellationToken) => Task.FromResult(SubscriberClient.Reply.Nack);

                // TODO: insert errors into a database
                return (msg, cancellationToken) =>
                {
                    m_logger.Error(e, $"Fatally could not process message {msg.MessageId}.");
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                };
            }
        }

        private static bool IsLikelyTemporaryFault(Exception exception)
        {
            return exception switch
            {
                HttpRequestException _ => true,
                InvalidOperationException _ => false,
                _ => true
            };
        }

        private Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> QueueDonorsForImportHandler() => async (msg, cancellationToken) =>
        {
            m_logger.Info("Getting all Donors from Denari...");
            var publishTasks = new List<Task<string>>();

            try
            {
                await foreach (var donor in m_drapiService.GetDonorsAsync(cancellationToken))
                {
                    m_logger.Debug($"Publishing import events for donor #{donor.Account}");
                    var json = JsonConvert.SerializeObject(donor);

                    publishTasks.Add(m_googleCloudPubSubService.PublishMessageAsync(Topics.HubspotImportFromDonor, json));
                }

                if (cancellationToken.IsCancellationRequested)
                    return SubscriberClient.Reply.Nack;

                await Task.WhenAll(publishTasks);
                m_logger.Info("Finished getting all Donors from Denari for import.");
                return SubscriberClient.Reply.Ack;
            }
            catch (Exception e)
            {
                m_logger.Error(e);
                return SubscriberClient.Reply.Nack;
            }
        };

        private Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> ImportCompanyFromDonorHandler() => async (msg, cancellationToken) =>
        {
            var donor = JsonConvert.DeserializeObject<Donor>(msg.Data.ToStringUtf8());

            if ((!donor.Email.IsValidEmailAddress() && !donor.Email2.IsValidEmailAddress())
                || (donor.Email.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email))
                || (donor.Email2.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email2)))
            {
                m_logger.Debug($"Importing company from donor #{donor.Account}");
                try
                {
                    await m_hubspotService.CreateOrUpdateCompanyAsync(IntegrationMapper.MapDonorToCompany(donor), cancellationToken);
                }
                catch (Exception e)
                {
                    m_logger.Error(e, $"Error creating or updating company in Hubspot for donor {donor.Account}.");
                    return SubscriberClient.Reply.Nack;
                }
            }

            return SubscriberClient.Reply.Ack;
        };

        private Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> ImportContactFromDonorHandler() => async (msg, cancellationToken) =>
        {
            var donor = JsonConvert.DeserializeObject<Donor>(msg.Data.ToStringUtf8());
            if (donor.Email.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email))
            {
                m_logger.Debug($"Importing contact from donor #{donor.Account} for email {donor.Email}");
                try
                {
                    await m_hubspotService.CreateOrUpdateContactAsync(IntegrationMapper.MapDonorToContact(donor, donor.Email));
                }
                catch (Exception e)
                {
                    m_logger.Error(e, $"Error creating or updating contact in Hubspot for email {donor.Email}.");
                    return SubscriberClient.Reply.Nack;
                }
            }

            if (donor.Email2.IsValidEmailAddress() && !await IsEmailOptedOutAsync(donor.Email2))
            {
                m_logger.Debug($"Importing contact from donor #{donor.Account} for email {donor.Email2}");
                try
                {
                    await m_hubspotService.CreateOrUpdateContactAsync(IntegrationMapper.MapDonorToContact(donor, donor.Email2));
                }
                catch (Exception e)
                {
                    m_logger.Error(e, $"Error creating or updating contact in Hubspot for email {donor.Email2}.");
                    return SubscriberClient.Reply.Nack;
                }
            }

            return SubscriberClient.Reply.Ack;
        };

        private async Task<bool> IsEmailOptedOutAsync(string? email)
        {
            if (email == null)
                return true;

            IReadOnlyList<string> optedOutList = await m_cache.GetOrCreateAsync(OptOutCacheKey, async entry =>
            {
                m_logger.Info("Caching email blacklist for 120 minutes");
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(120));
                return await m_hubspotService.GetOptedOutEmailsAsync();
            });

            return optedOutList.Contains(email);
        }
    }
}
