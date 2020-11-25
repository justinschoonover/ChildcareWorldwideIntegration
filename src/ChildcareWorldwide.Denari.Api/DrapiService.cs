using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Denari.Api.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using RateLimiter;

namespace ChildcareWorldwide.Denari.Api
{
	public sealed class DrapiService : IDrapiService, IDisposable
	{
		private readonly HttpClient m_client;
		private readonly Logger m_logger;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
		public DrapiService(IConfiguration configuration)
		{
			m_logger = LogManager.GetCurrentClassLogger();
			m_client = new HttpClient(new DrapiHttpClientHandler()) { BaseAddress = new Uri("https://api.denarionline.com/v1/api/") };
			m_client.DefaultRequestHeaders.Add("drapi-authorization", configuration["DenariApiKey"]);
		}

		public void Dispose() => m_client.Dispose();

		public async Task<(Donor? donor, string? rawJson)> GetDonorByAccountAsync(string accountNumber, CancellationToken cancellationToken = default)
		{
			m_logger.Info($"Getting Denari donor information for donor # \"{accountNumber}\"");

			using var filterJson = SerializeJsonForDrapi(new DonorList<Donor>
			{
				PageSize = 1,
				PageCount = 0,
				CurrentPage = 0,
				Order = string.Empty,
				Filter = new DrapiFilter
				{
					FilterTree = new List<BooleanCondition>
					{
						new BooleanCondition
						{
							JoinAs = "AND",
							TermList = new List<BooleanTerm>
							{
								new BooleanTerm
								{
									Field = "Donor_Account",
									LogicalOperator = "eq",
									Target = new List<string>
									{
										accountNumber,
									},
									JoinAs = "AND",
								},
							},
						},
					},
				},
			});
			m_logger.Debug("Requesting from Denari API...");
			var response = await m_client.PostAsync("Donor/firstpage", filterJson, cancellationToken);
			m_logger.Trace(response);
			response.EnsureSuccessStatusCode();

			string responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
			m_logger.Trace(responseJson);
			var donors = JsonConvert.DeserializeObject<DonorList<Donor>>(responseJson);
			return (donors.Data.FirstOrDefault(), responseJson);
		}

		public async IAsyncEnumerable<Donor> GetDonorsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			const int pageSize = 1000;
			int currentPage = 0;
			int pageCount = 1;

			while (currentPage < pageCount)
			{
				if (cancellationToken.IsCancellationRequested)
					break;

				using var json = SerializeJsonForDrapi(new DonorList<Donor>
				{
					PageSize = pageSize,
					PageCount = pageCount,
					CurrentPage = currentPage,
					Order = string.Empty,
					Filter = new DrapiFilter(),
				});

				string endpoint = currentPage == 0
					? "Donor/firstpage"
					: "Donor/nextpage";
				var response = await m_client.PostAsync(endpoint, json, cancellationToken);
				response.EnsureSuccessStatusCode();

				var donorList = JsonConvert.DeserializeObject<DonorList<Donor>>(await response.Content.ReadAsStringAsync(cancellationToken));
				pageCount = donorList.PageCount;
				currentPage = donorList.CurrentPage;
				foreach (var donor in donorList.Data.Where(d => d.Account != "-1"))
					yield return donor;
			}
		}

		public async IAsyncEnumerable<DonorClassification> GetClassificationsForDonorAsync(string donorKey, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			m_logger.Info($"Getting Denari donor classifications for donor ID \"{donorKey}\"");

			int currentPage = 0;
			int pageCount = 1;

			while (currentPage < pageCount)
			{
				if (cancellationToken.IsCancellationRequested)
					break;

				var response = await m_client.GetAsync($"Donor/{donorKey}/classification", cancellationToken);
				response.EnsureSuccessStatusCode();

				var donorList = JsonConvert.DeserializeObject<DonorList<DonorClassification>>(await response.Content.ReadAsStringAsync(cancellationToken));
				pageCount = donorList.PageCount;
				currentPage = donorList.CurrentPage;
				foreach (var donorClassification in donorList.Data)
					yield return donorClassification;
			}
		}

		private static StringContent SerializeJsonForDrapi<T>(DonorList<T> requestFilter)
		{
			string json = JsonConvert.SerializeObject(
				requestFilter,
				Formatting.Indented,
				new JsonSerializerSettings
				{
					ContractResolver = new DefaultContractResolver
					{
						NamingStrategy = new CamelCaseNamingStrategy(),
					},
					NullValueHandling = NullValueHandling.Ignore,
				});

			// note the awkward JSON syntax - wrapped in quotes and using single quotes for identifiers
			return new StringContent($"\"{json.Replace("\"", "'", StringComparison.InvariantCultureIgnoreCase)}\"", Encoding.UTF8, "application/json");
		}

		private sealed class DrapiHttpClientHandler : DelegatingHandler
		{
			private readonly SimpleRateLimiter m_rateLimiter;

			public DrapiHttpClientHandler()
			{
				InnerHandler = new HttpClientHandler();

				// Denari API doesn't publish rate limits. Impose a limit of 100 requests every 10 seconds anyways.
				m_rateLimiter = SimpleRateLimiter.MaxRequestsPerInterval(100, TimeSpan.FromSeconds(10));
			}

			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				await m_rateLimiter.WaitForReady(cancellationToken);
				return await base.SendAsync(request, cancellationToken);
			}
		}
	}
}
