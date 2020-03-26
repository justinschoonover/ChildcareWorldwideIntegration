using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Helpers;
using Google.Cloud.SecretManager.V1Beta1;

namespace ChildcareWorldwide.Google.Api
{
    internal sealed class GoogleSecretManagerService
    {
        private readonly SecretManagerServiceClient client;
        private readonly string projectId;

        public GoogleSecretManagerService()
        {
            client = SecretManagerServiceClient.Create();
            projectId = GoogleComputeEngineHelper.GetCurrentProjectId();
        }

        internal async IAsyncEnumerable<string> ListSecretsAsync()
        {
            var response = client.ListSecretsAsync($"projects/{projectId}");
            await foreach (Secret item in response)
            {
                yield return item.SecretName.SecretId;
            }
        }

        internal async Task<string> GetSecretAsync(string secretId) =>
            await AccessSecretVersion(secretId, "latest");

        private async Task<string> AccessSecretVersion(string secretId, string secretVersion)
        {
            try
            {
                var request = new AccessSecretVersionRequest
                {
                    SecretVersionName = new SecretVersionName(projectId, secretId, secretVersion),
                };

                var response = await client.AccessSecretVersionAsync(request);
                return response.Payload.Data.ToStringUtf8();
            }
            catch (InvalidOperationException)
            {
                return string.Empty;
            }
        }
    }
}
