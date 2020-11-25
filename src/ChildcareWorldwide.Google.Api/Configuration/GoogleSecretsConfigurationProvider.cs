using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Google.Api.Configuration
{
	public sealed class GoogleSecretsConfigurationProvider : ConfigurationProvider
	{
		private readonly GoogleSecretManagerService m_googleSecretManagerService;

		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleSecretsConfigurationProvider"/> class.
		/// </summary>
		public GoogleSecretsConfigurationProvider()
		{
			m_googleSecretManagerService = new GoogleSecretManagerService();
		}

		/// <summary>
		/// Loads the environment variables.
		/// </summary>
		public override void Load() => LoadAsync().GetAwaiter().GetResult();

		private async Task LoadAsync()
		{
			await foreach (string? secretId in m_googleSecretManagerService.ListSecretsAsync())
			{
				string? secretValue = await m_googleSecretManagerService.GetSecretAsync(secretId);
				Data.Add(secretId, secretValue);
			}
		}
	}
}
