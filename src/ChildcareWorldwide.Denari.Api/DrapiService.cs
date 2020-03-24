using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ChildcareWorldwide.Denari.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChildcareWorldwide.Denari.Api
{
    public sealed class DrapiService : IDrapiService
    {
        private readonly Uri m_baseUri = new Uri("https://api.denarionline.com/v1/api/");
        private readonly string m_denariApiKey;

        public DrapiService(string denariApiKey = "")
        {
            m_denariApiKey = denariApiKey;
        }

        public async IAsyncEnumerable<Donor> GetDonorsAsync()
        {
            using HttpClient client = GetClient();
            var firstPageRequest = new DonorList
            {
                PageSize = 500,
                PageCount = 0,
                CurrentPage = 1,
                Order = string.Empty,
                Filter = new DrapiFilter(),
            };
            var json = JsonConvert.SerializeObject(firstPageRequest, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                NullValueHandling = NullValueHandling.Ignore,
            });

            // note the awkward JSON syntax - wrapped in quotes and using single quotes for identifiers
            var response = await client.PostAsync("Donor/firstpage", new StringContent($"\"{json.Replace("\"", "'", StringComparison.InvariantCultureIgnoreCase)}\"", Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var donors = JsonConvert.DeserializeObject<DonorList>(await response.Content.ReadAsStringAsync());
            foreach (var donor in donors.Data)
            {
                yield return donor;
            }
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient() { BaseAddress = m_baseUri };
            client.DefaultRequestHeaders.Add("drapi-authorization", m_denariApiKey);
            return client;
        }
    }
}
