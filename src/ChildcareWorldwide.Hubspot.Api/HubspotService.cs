using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Hubspot.Api.Helpers;
using ChildcareWorldwide.Hubspot.Api.Mappers;
using ChildcareWorldwide.Hubspot.Api.Models;
using ChildcareWorldwide.Hubspot.Api.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RateLimiter;

namespace ChildcareWorldwide.Hubspot.Api
{
    public sealed class HubspotService : IHubspotService, IDisposable
    {
        private readonly HttpClient m_client = default!;
        private readonly SimpleRateLimiter m_rateLimiter = default!;
        private readonly Random m_jitterer = default!;

        private readonly ConcurrentDictionary<string, Contact> m_contactsByEmail = new ConcurrentDictionary<string, Contact>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
        public HubspotService(IConfiguration configuration)
        {
            m_client = new HttpClient(new HubspotHttpClientHandler(configuration["HubspotApiKey"])) { BaseAddress = new Uri("https://api.hubapi.com/") };

            // Hubspot API has a global limit of 100 requests every 10 seconds
            m_rateLimiter = SimpleRateLimiter.MaxRequestsPerInterval(100, TimeSpan.FromSeconds(10));
            m_jitterer = new Random();

            m_client.DefaultRequestHeaders.Accept.Clear();
            m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            m_rateLimiter.Dispose();
            m_client.Dispose();
        }

        #region CRM Objects API - Companies https://developers.hubspot.com/docs/api/crm/companies

        public async Task<Company?> GetCompanyByDenariAccountIdAsync(string accountId, CancellationToken cancellationToken = default)
        {
            var filter = new CrmSearchOptions
            {
                FilterGroups =
                {
                    new CrmSearchFilterGroups
                    {
                        Filters =
                        {
                            new CrmSearchFilter
                            {
                                PropertyName = "account_id",
                                Operator = "EQ",
                                Value = accountId,
                            },
                        },
                    },
                },
                Properties =
                {
                    "account_id",
                },
                Limit = 1,
            };

            var json = JsonConvert.SerializeObject(filter, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
            });

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync("/crm/v3/objects/companies/search", content, cancellationToken),
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await response.EnsureSuccessStatusCodeWithResponseBodyInException();

            var searchResults = JsonConvert.DeserializeObject<GetCrmObjectsResult>(await response.Content.ReadAsStringAsync());
            if (searchResults == null)
                return null;

            return DomainModelMapper.MapDomainModel<Company>(searchResults.Results.FirstOrDefault());
        }

        public async Task<Company> CreateOrUpdateCompanyAsync(Company company, CancellationToken cancellationToken = default)
        {
            var existingCompany = await GetCompanyByDenariAccountIdAsync(company.DenariAccountId, cancellationToken);
            return DomainModelMapper.MapDomainModel<Company>(existingCompany != null
                ? await UpdateCompanyAsync(company, existingCompany, cancellationToken)
                : await CreateCompanyAsync(company, cancellationToken)) !;
        }

        #endregion

        #region CRM Objects API - Contacts https://developers.hubspot.com/docs/api/crm/contacts

        public async Task<Contact?> GetContactByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var filter = new CrmSearchOptions
            {
                FilterGroups =
                {
                    new CrmSearchFilterGroups
                    {
                        Filters =
                        {
                            new CrmSearchFilter
                            {
                                PropertyName = "email",
                                Operator = "EQ",
                                Value = email,
                            },
                        },
                    },
                },
                Properties =
                {
                    "email",
                },
                Limit = 1,
            };

            var json = JsonConvert.SerializeObject(filter, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
            });

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync("/crm/v3/objects/contacts/search", content, cancellationToken),
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await response.EnsureSuccessStatusCodeWithResponseBodyInException();

            var searchResults = JsonConvert.DeserializeObject<GetCrmObjectsResult>(await response.Content.ReadAsStringAsync());
            if (searchResults == null)
                return null;

            return DomainModelMapper.MapDomainModel<Contact>(searchResults.Results.FirstOrDefault());
        }

        public async Task<Contact> CreateOrUpdateContactAsync(Contact contact, CancellationToken cancellationToken = default)
        {
            var inProcessContact = m_contactsByEmail.GetOrAdd(contact.Email, contact);
            if (inProcessContact != contact)
                throw new InvalidOperationException($"Cannot create contact for email {contact.Email} for donor {contact.DenariAccountId}, because that email is already being imported into Hubspot for donor {inProcessContact.DenariAccountId}.");

            var existingContact = await GetContactByEmailAsync(contact.Email, cancellationToken);
            if (existingContact?.DenariAccountId != contact.DenariAccountId && existingContact?.DenariAccountId != null)
                throw new InvalidOperationException($"Cannot create contact for email {contact.Email} for donor {contact.DenariAccountId}, because that email already exists in Hubspot and belongs to donor {existingContact.DenariAccountId}.");

            return DomainModelMapper.MapDomainModel<Contact>(existingContact != null
                ? await UpdateContactAsync(contact, existingContact, cancellationToken)
                : await CreateContactAsync(contact, cancellationToken)) !;
        }

        #endregion

        #region Email Subscription API https://developers.hubspot.com/docs/methods/email/email_subscriptions_overview

        public async Task<IReadOnlyList<string>> GetOptedOutEmailsAsync(CancellationToken cancellationToken = default)
        {
            var optedOutEmails = await ApiPagingUtility.IterateAsync(async offset =>
            {
                var response = await RequestWithRetriesAsync(
                    async cancellationToken => await m_client.GetAsync($"/email/public/v1/subscriptions/timeline?offset={offset}&limit=1000&changeType=SUBSCRIPTION_STATUS", cancellationToken),
                    cancellationToken);
                await response.EnsureSuccessStatusCodeWithResponseBodyInException();

                var emailTimeline = JsonConvert.DeserializeObject<GetEmailTimelineResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                }) !;

                return new PageOffsetSummary<string>(
                    emailTimeline.Timeline.Where(t => t.Changes.Any(c => c.Change == "UNSUBSCRIBED")).Select(t => t.Recipient).ToList(),
                    emailTimeline.Offset!,
                    emailTimeline.HasMore);
            });

            return optedOutEmails.Distinct().ToList();
        }

        #endregion

        #region CRM Properties API https://developers.hubspot.com/docs/api/crm/properties

        #region Property Groups

        public async IAsyncEnumerable<CrmPropertyGroup> ListContactPropertyGroupsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var contactGroup in ListCrmPropertyGroupsAsync("contact", cancellationToken))
                yield return contactGroup;
        }

        public async IAsyncEnumerable<CrmPropertyGroup> ListCompanyPropertyGroupsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var contactGroup in ListCrmPropertyGroupsAsync("company", cancellationToken))
                yield return contactGroup;
        }

        public async Task<CrmPropertyGroup?> GetContactPropertyGroupAsync(string groupName, CancellationToken cancellationToken = default) =>
            await GetCrmPropertyGroupAsync("contact", groupName, cancellationToken);
        public async Task<CrmPropertyGroup?> GetCompanyPropertyGroupAsync(string groupName, CancellationToken cancellationToken = default) =>
            await GetCrmPropertyGroupAsync("company", groupName, cancellationToken);

        public async Task CreateContactPropertyGroupAsync(CrmPropertyGroup propertyGroup, CancellationToken cancellationToken = default)
        {
            // this isn't a field you set on CREATE
            propertyGroup.Archived = null;

            await CreateCrmPropertyGroupAsync("contact", propertyGroup, cancellationToken);
        }

        public async Task CreateCompanyPropertyGroupAsync(CrmPropertyGroup propertyGroup, CancellationToken cancellationToken = default)
        {
            // this isn't a field you set on CREATE
            propertyGroup.Archived = null;

            await CreateCrmPropertyGroupAsync("company", propertyGroup, cancellationToken);
        }

        #endregion

        #region Properties

        public async IAsyncEnumerable<CrmProperty> ListContactPropertiesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var contact in ListCrmPropertiesAsync("contact", cancellationToken))
                yield return contact;
        }

        public async IAsyncEnumerable<CrmProperty> ListCompanyPropertiesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var company in ListCrmPropertiesAsync("company", cancellationToken))
                yield return company;
        }

        public async Task<CrmProperty?> GetContactPropertyAsync(string propertyName, CancellationToken cancellationToken = default) =>
            await GetCrmPropertyAsync("contact", propertyName, cancellationToken);

        public async Task<CrmProperty?> GetCompanyPropertyAsync(string propertyName, CancellationToken cancellationToken = default) =>
            await GetCrmPropertyAsync("company", propertyName, cancellationToken);

        public async Task CreateContactPropertyAsync(CrmProperty contactProperty, CancellationToken cancellationToken = default)
        {
            // this isn't a field you set on CREATE
            contactProperty.CreatedAt = null;

            await CreateCrmPropertyAsync("contact", contactProperty, cancellationToken);
        }

        public async Task CreateCompanyPropertyAsync(CrmProperty companyProperty, CancellationToken cancellationToken = default)
        {
            // this isn't a field you set on CREATE
            companyProperty.CreatedAt = null;

            await CreateCrmPropertyAsync("company", companyProperty, cancellationToken);
        }

        #endregion

        #endregion

        private async Task<CrmObject> CreateContactAsync(Contact contact, CancellationToken cancellationToken)
        {
            using var content = new StringContent(DomainModelMapper.GetPropertiesForCreate(contact), Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync("/crm/v3/objects/contacts", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();

            return JsonConvert.DeserializeObject<CrmObject>(await response.Content.ReadAsStringAsync());
        }

        private async Task<CrmObject> UpdateContactAsync(Contact updatedContact, Contact existingContact, CancellationToken cancellationToken)
        {
            using var content = new StringContent(DomainModelMapper.GetPropertiesForUpdate(updatedContact, existingContact), Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PatchAsync($"/crm/v3/objects/contacts/{existingContact.Id}", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
            return JsonConvert.DeserializeObject<CrmObject>(await response.Content.ReadAsStringAsync());
        }

        private async Task<CrmObject> CreateCompanyAsync(Company company, CancellationToken cancellationToken)
        {
            using var content = new StringContent(DomainModelMapper.GetPropertiesForCreate(company), Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync("/crm/v3/objects/companies", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
            return JsonConvert.DeserializeObject<CrmObject>(await response.Content.ReadAsStringAsync());
        }

        private async Task<CrmObject> UpdateCompanyAsync(Company updatedCompany, Company existingCompany, CancellationToken cancellationToken)
        {
            using var content = new StringContent(DomainModelMapper.GetPropertiesForUpdate(updatedCompany, existingCompany), Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PatchAsync($"/crm/v3/objects/companies/{existingCompany.Id}", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
            return JsonConvert.DeserializeObject<CrmObject>(await response.Content.ReadAsStringAsync());
        }

        private async IAsyncEnumerable<CrmPropertyGroup> ListCrmPropertyGroupsAsync(string endpoint, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.GetAsync($"/crm/v3/properties/{endpoint}/groups", cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();

            var propertyGroups = JsonConvert.DeserializeObject<GetAllPropertyGroupsResult>(await response.Content.ReadAsStringAsync());
            foreach (var propertyGroup in propertyGroups.Results)
                yield return propertyGroup;
        }

        private async Task<CrmPropertyGroup?> GetCrmPropertyGroupAsync(string endpoint, string groupName, CancellationToken cancellationToken)
        {
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.GetAsync($"/crm/v3/properties/{endpoint}/groups/{groupName}", cancellationToken),
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
            return JsonConvert.DeserializeObject<CrmPropertyGroup>(await response.Content.ReadAsStringAsync());
        }

        private async Task CreateCrmPropertyGroupAsync(string endpoint, CrmPropertyGroup propertyGroup, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(propertyGroup, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
            });

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync($"/crm/v3/properties/{endpoint}/groups", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
        }

        private async IAsyncEnumerable<CrmProperty> ListCrmPropertiesAsync(string endpoint, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.GetAsync($"/crm/v3/properties/{endpoint}", cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();

            var properties = JsonConvert.DeserializeObject<GetAllPropertiesResult>(await response.Content.ReadAsStringAsync());
            foreach (var property in properties.Results)
                yield return property;
        }

        private async Task<CrmProperty?> GetCrmPropertyAsync(string endpoint, string propertyName, CancellationToken cancellationToken)
        {
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.GetAsync($"/crm/v3/properties/{endpoint}/{propertyName}", cancellationToken),
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
            return JsonConvert.DeserializeObject<CrmProperty>(await response.Content.ReadAsStringAsync());
        }

        private async Task CreateCrmPropertyAsync(string endpoint, CrmProperty contactProperty, CancellationToken cancellationToken)
        {
            // this isn't a field you set on CREATE
            contactProperty.CreatedAt = null;

            var json = JsonConvert.SerializeObject(contactProperty, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
            });

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await RequestWithRetriesAsync(
                async cancellationToken => await m_client.PostAsync($"/crm/v3/properties/{endpoint}", content, cancellationToken),
                cancellationToken);
            await response.EnsureSuccessStatusCodeWithResponseBodyInException();
        }

        private async Task<HttpResponseMessage> RequestWithRetriesAsync(Func<CancellationToken, Task<HttpResponseMessage>> request, CancellationToken cancellationToken)
        {
            int retryAttempt = 0;

            await m_rateLimiter.WaitForReady(cancellationToken);
            var response = await request(cancellationToken);

            // API responds with "429 Too Many Requests" if any of the rate limits are exceeded
            // API response occasionally with "502 Bad Gateway" due to unknown issues on Hubspot's end
            while (response.StatusCode == HttpStatusCode.TooManyRequests || response.StatusCode == HttpStatusCode.BadGateway)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                // retry if we're rate limited (after a delay)
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryAttempt++)) + TimeSpan.FromMilliseconds(m_jitterer.Next(0, 100)), cancellationToken);
                response = await request(cancellationToken);
            }

            return response;
        }

        private sealed class HubspotHttpClientHandler : DelegatingHandler
        {
            private readonly Dictionary<string, string> m_authParams;

            public HubspotHttpClientHandler(string apiKey)
            {
                InnerHandler = new HttpClientHandler();
                m_authParams = new Dictionary<string, string>
                {
                    { "hapikey", apiKey },
                };
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.RequestUri = new Uri(QueryHelpers.AddQueryString(request.RequestUri.AbsoluteUri, m_authParams));
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
