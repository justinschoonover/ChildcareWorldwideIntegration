using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Google.Api.Configuration;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldWide.TestFixtures.Integration
{
    public abstract class TestFixtureBase
    {
        protected DrapiService DenariService { get; private set; } = default!;
        protected HubspotService HubspotService { get; private set; } = default!;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
        public void OneTimeSetupBase()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<TestFixtureBase>();
            builder.AddGoogleSecretsConfiguration();
            IConfiguration configuration = builder.Build();

            DenariService = new DrapiService(configuration);
            HubspotService = new HubspotService(configuration, new MemoryCache(new MemoryCacheOptions()));
        }
    }
}
