using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Google.Api.Configuration
{
    public sealed class GoogleSecretsConfigurationProvider : ConfigurationProvider
    {
        private readonly GoogleSecretManagerService googleSecretManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleSecretsConfigurationProvider"/> class.
        /// </summary>
        public GoogleSecretsConfigurationProvider()
        {
            googleSecretManagerService = new GoogleSecretManagerService();
        }

        /// <summary>
        /// Loads the environment variables.
        /// </summary>
        public override void Load() => LoadAsync().GetAwaiter().GetResult();

        private async Task LoadAsync()
        {
            await foreach (string secretId in googleSecretManagerService.ListSecretsAsync())
            {
                string secretValue = await googleSecretManagerService.GetSecretAsync(secretId);
                Data.Add(secretId, secretValue);
            }
        }
    }
}
