using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Configuration;
using ChildcareWorldwide.Hubspot.Api;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Tools.CloneHubspotProperties
{
    [SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable")]
    public class Program
    {
        private static IConfiguration Configuration { get; set; } = default!;

        public static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: CloneHubspotProperties.exe [Production API key] [Sandbox API key]");
                return;
            }

            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Program>();
            builder.AddGoogleSecretsConfiguration();
            Configuration = builder.Build();

            await CloneHubspotProperties(args[0], args[1]);
        }

        public static async Task CloneHubspotProperties(string productionApiKey, string sandboxApiKey)
        {
            using var productionService = new HubspotService(productionApiKey);
            using var sandboxService = new HubspotService(sandboxApiKey);

            Console.WriteLine("Cloning Custom Contact Property Groups...");
            await foreach (var propertyGroup in productionService.ListContactPropertyGroupsAsync())
            {
                if (await sandboxService.GetContactPropertyGroupAsync(propertyGroup.Name) == null)
                {
                    Console.WriteLine(propertyGroup.Name);
                    await sandboxService.CreateContactPropertyGroupAsync(propertyGroup);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Cloning Custom Contact Properties...");
            await foreach (var property in productionService.ListContactPropertiesAsync())
            {
                if (property.Name.StartsWith("hs_", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (await sandboxService.GetContactPropertyAsync(property.Name) == null)
                {
                    Console.WriteLine(property.Name);
                    await sandboxService.CreateContactPropertyAsync(property);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Cloning Custom Company Property Groups...");
            await foreach (var propertyGroup in productionService.ListCompanyPropertyGroupsAsync())
            {
                if (await sandboxService.GetCompanyPropertyGroupAsync(propertyGroup.Name) == null)
                {
                    Console.WriteLine(propertyGroup.Name);
                    await sandboxService.CreateCompanyPropertyGroupAsync(propertyGroup);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Cloning Custom Company Properties...");
            await foreach (var property in productionService.ListCompanyPropertiesAsync())
            {
                if (property.Name.StartsWith("hs_", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (await sandboxService.GetCompanyPropertyAsync(property.Name) == null)
                {
                    Console.WriteLine(property.Name);
                    await sandboxService.CreateCompanyPropertyAsync(property);
                }
            }
        }
    }
}
