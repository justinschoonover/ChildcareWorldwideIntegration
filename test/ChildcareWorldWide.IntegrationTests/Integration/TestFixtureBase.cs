using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Google.Api.Configuration;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldWide.TestFixtures.Integration
{
	public abstract class TestFixtureBase
	{
		private static HubspotService? s_hubspotService;

		protected IDrapiService DenariService { get; private set; } = null!;
		protected IHubspotService HubspotService { get; private set; } = null!;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
		public static HubspotService GetHubspotService()
		{
			if (s_hubspotService != null)
				return s_hubspotService;

			var builder = new ConfigurationBuilder();
			builder.AddUserSecrets<TestFixtureBase>();
			builder.AddGoogleSecretsConfiguration();
			IConfiguration configuration = builder.Build();
			s_hubspotService = new HubspotService(configuration, new MemoryCache(new MemoryCacheOptions()));
			return s_hubspotService;
		}

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
