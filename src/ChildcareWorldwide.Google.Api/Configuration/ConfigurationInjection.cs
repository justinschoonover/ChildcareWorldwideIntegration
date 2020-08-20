using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;

[assembly: HostingStartup(typeof(ChildcareWorldwide.Google.Api.Configuration.ConfigurationInjection))]
namespace ChildcareWorldwide.Google.Api.Configuration
{
    public sealed class ConfigurationInjection : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            var logger = LogManager.GetCurrentClassLogger();

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(GetOAuthSettings());
            });
        }

        private static Dictionary<string, string> GetOAuthSettings()
        {
            var secretService = new GoogleSecretManagerService();
            const string oauthClientIdSecretName = "Authentication_Google_ClientId";
            const string oauthClientSecretSecretName = "Authentication_Google_ClientSecret";

            return new Dictionary<string, string>
                {
                    { oauthClientIdSecretName, secretService.GetSecretAsync(oauthClientIdSecretName).GetAwaiter().GetResult() },
                    { oauthClientSecretSecretName, secretService.GetSecretAsync(oauthClientSecretSecretName).GetAwaiter().GetResult() },
                };
        }
    }
}
