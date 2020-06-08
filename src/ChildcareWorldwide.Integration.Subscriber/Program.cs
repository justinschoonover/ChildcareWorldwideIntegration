using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Google.Api;
using ChildcareWorldwide.Google.Api.Configuration;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace ChildcareWorldwide.Integration.Subscriber
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddGoogleSecretsConfiguration();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDrapiService, DrapiService>();
                    services.AddSingleton<IHubspotService, HubspotService>();
                    services.AddSingleton<IGoogleCloudPubSubService, GoogleCloudPubSubService>();
                    services.AddMemoryCache();
                    services.AddHostedService<Worker>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}
