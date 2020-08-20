using Microsoft.AspNetCore.Hosting;
using NLog;

[assembly: HostingStartup(typeof(ChildcareWorldwide.Google.Api.Configuration.ConfigurationInjection))]
namespace ChildcareWorldwide.Google.Api.Configuration
{
    public sealed class ConfigurationInjection : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Injecting Google Secrets into configuration providers.");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddGoogleSecretsConfiguration();
            });
        }
    }
}
