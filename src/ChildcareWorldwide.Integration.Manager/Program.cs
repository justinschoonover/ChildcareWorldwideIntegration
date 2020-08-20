using System;
using ChildcareWorldwide.Google.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace ChildcareWorldwide.Integration.Manager
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Info("Starting ChildcareWorldwide.Integration.Manager");
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                LogManager.Flush(TimeSpan.FromSeconds(15));
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            string uri = $"http://0.0.0.0:{port}";

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "ChildcareWorldwide.Google.Api").UseStartup<Startup>().UseUrls(uri);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
        }
    }
}
