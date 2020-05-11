using System;
using System.Collections.Generic;
using System.Text;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Google.Api.Configuration;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldWide.IntegrationTests
{
    public abstract class TestFixtureBase
    {
        protected DrapiService DenariService { get; private set; } = default!;
        protected HubspotService HubspotService { get; private set; } = default!;

        private static IConfiguration Configuration { get; set; } = default!;

        public void OneTimeSetupBase()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<TestFixtureBase>();
            builder.AddGoogleSecretsConfiguration();
            Configuration = builder.Build();

            DenariService = new DrapiService(Configuration["DenariApiKey"]);
            HubspotService = new HubspotService(Configuration["HubspotApiKey"]);
        }
    }
}
