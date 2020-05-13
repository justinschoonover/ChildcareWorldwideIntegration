using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Hubspot.Api.Helpers;
using ChildcareWorldwide.Hubspot.Api.Mappers;
using ChildcareWorldwide.Hubspot.Api.Models;
using ChildcareWorldwide.Hubspot.Api.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChildcareWorldwide.Hubspot.Api
{
    public sealed class HubspotService : IHubspotService, IDisposable
    {
        private readonly HttpClient m_client = default!;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
        public HubspotService(string apiKey)
        {
            m_client = new HttpClient(new HubspotHttpClientHandler(apiKey)) { BaseAddress = new Uri("https://api.hubapi.com/") };

            m_client.DefaultRequestHeaders.Accept.Clear();
            m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            m_client.Dispose();
        }

        #region CRM Companies API https://developers.hubspot.com/docs/api/crm/companies

        public async Task<Company?> GetCompanyByDenariAccountIdAsync(string accountId)
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
            var response = await m_client.PostAsync("/crm/v3/objects/companies/search", content);
            response.EnsureSuccessStatusCode();

            var searchResults = JsonConvert.DeserializeObject<GetCrmObjectsResult>(await response.Content.ReadAsStringAsync());
            if (searchResults == null)
                return null;

            return CompanyMapper.MapCompany(searchResults.Results.FirstOrDefault());
        }

        public async Task CreateCompanyAsync(Company company)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region CRM Contacts API https://developers.hubspot.com/docs/api/crm/contacts

        public async Task<Contact> GetContactAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateContactAsync(Contact contact)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Email Subscription API https://developers.hubspot.com/docs/methods/email/email_subscriptions_overview

        public async Task<bool> IsEmailOptedOutAsync(string email)
        {
            var response = await m_client.GetAsync($"/email/public/v1/subscriptions/{email}");
            response.EnsureSuccessStatusCode();

            var emailSubscriptionStatus = JsonConvert.DeserializeObject<GetEmailSubscriptionStatusResult>(await response.Content.ReadAsStringAsync());
            return !emailSubscriptionStatus?.Subscribed ?? false;
        }

        #endregion

        #region CRM Properties API https://developers.hubspot.com/docs/api/crm/properties

        #region Property Groups

        public async IAsyncEnumerable<CrmPropertyGroup> ListContactPropertyGroupsAsync()
        {
            await foreach (var contactGroup in ListCrmPropertyGroupsAsync("contact"))
                yield return contactGroup;
        }

        public async IAsyncEnumerable<CrmPropertyGroup> ListCompanyPropertyGroupsAsync()
        {
            await foreach (var contactGroup in ListCrmPropertyGroupsAsync("company"))
                yield return contactGroup;
        }

        public async Task<CrmPropertyGroup?> GetContactPropertyGroupAsync(string groupName) => await GetCrmPropertyGroupAsync("contact", groupName);
        public async Task<CrmPropertyGroup?> GetCompanyPropertyGroupAsync(string groupName) => await GetCrmPropertyGroupAsync("company", groupName);

        public async Task CreateContactPropertyGroupAsync(CrmPropertyGroup propertyGroup)
        {
            // this isn't a field you set on CREATE
            propertyGroup.Archived = null;

            await CreateCrmPropertyGroupAsync("contact", propertyGroup);
        }

        public async Task CreateCompanyPropertyGroupAsync(CrmPropertyGroup propertyGroup)
        {
            // this isn't a field you set on CREATE
            propertyGroup.Archived = null;

            await CreateCrmPropertyGroupAsync("company", propertyGroup);
        }

        #endregion

        #region Properties

        public async IAsyncEnumerable<CrmProperty> ListContactPropertiesAsync()
        {
            await foreach (var contact in ListCrmPropertiesAsync("contact"))
                yield return contact;
        }

        public async IAsyncEnumerable<CrmProperty> ListCompanyPropertiesAsync()
        {
            await foreach (var company in ListCrmPropertiesAsync("company"))
                yield return company;
        }

        public async Task<CrmProperty?> GetContactPropertyAsync(string propertyName) => await GetCrmPropertyAsync("contact", propertyName);

        public async Task<CrmProperty?> GetCompanyPropertyAsync(string propertyName) => await GetCrmPropertyAsync("company", propertyName);

        public async Task CreateContactPropertyAsync(CrmProperty contactProperty)
        {
            // this isn't a field you set on CREATE
            contactProperty.CreatedAt = null;

            await CreateCrmPropertyAsync("contact", contactProperty);
        }

        public async Task CreateCompanyPropertyAsync(CrmProperty companyProperty)
        {
            // this isn't a field you set on CREATE
            companyProperty.CreatedAt = null;

            await CreateCrmPropertyAsync("company", companyProperty);
        }

        #endregion

        private async IAsyncEnumerable<CrmPropertyGroup> ListCrmPropertyGroupsAsync(string endpoint)
        {
            var response = await m_client.GetAsync($"/crm/v3/properties/{endpoint}/groups");
            response.EnsureSuccessStatusCode();
            var propertyGroups = JsonConvert.DeserializeObject<GetAllPropertyGroupsResult>(await response.Content.ReadAsStringAsync());
            foreach (var propertyGroup in propertyGroups.Results)
                yield return propertyGroup;
        }

        private async Task<CrmPropertyGroup?> GetCrmPropertyGroupAsync(string endpoint, string groupName)
        {
            var response = await m_client.GetAsync($"/crm/v3/properties/{endpoint}/groups/{groupName}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<CrmPropertyGroup>(await response.Content.ReadAsStringAsync());
        }

        private async Task CreateCrmPropertyGroupAsync(string endpoint, CrmPropertyGroup propertyGroup)
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
            var response = await m_client.PostAsync($"/crm/v3/properties/{endpoint}/groups", content);
            response.EnsureSuccessStatusCode();
        }

        private async IAsyncEnumerable<CrmProperty> ListCrmPropertiesAsync(string endpoint)
        {
            var response = await m_client.GetAsync($"/crm/v3/properties/{endpoint}");
            response.EnsureSuccessStatusCode();
            var properties = JsonConvert.DeserializeObject<GetAllPropertiesResult>(await response.Content.ReadAsStringAsync());
            foreach (var property in properties.Results)
                yield return property;
        }

        private async Task<CrmProperty?> GetCrmPropertyAsync(string endpoint, string propertyName)
        {
            var response = await m_client.GetAsync($"/crm/v3/properties/{endpoint}/{propertyName}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<CrmProperty>(await response.Content.ReadAsStringAsync());
        }

        private async Task CreateCrmPropertyAsync(string endpoint, CrmProperty contactProperty)
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
            var response = await m_client.PostAsync($"/crm/v3/properties/{endpoint}", content);
            response.EnsureSuccessStatusCode();
        }

        #endregion

        private sealed class HubspotHttpClientHandler : DelegatingHandler
        {
            private readonly RateLimiter m_rateLimiter;
            private readonly Dictionary<string, string> m_authParams;

            public HubspotHttpClientHandler(string apiKey)
            {
                InnerHandler = new HttpClientHandler();

                // Hubspot API has a global limit of 100 requests every 10 seconds
                m_rateLimiter = RateLimiter.MaxRequestsPerInterval(100, TimeSpan.FromSeconds(10));
                m_authParams = new Dictionary<string, string>
                {
                    { "hapikey", apiKey },
                };
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                await m_rateLimiter.WaitForReady();
                request.RequestUri = new Uri(QueryHelpers.AddQueryString(request.RequestUri.AbsoluteUri, m_authParams));
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
