using System.Collections.Generic;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Helpers;
using Google.Cloud.SecretManager.V1;
using Grpc.Core;
using NLog;

namespace ChildcareWorldwide.Google.Api
{
    internal sealed class GoogleSecretManagerService
    {
        private readonly SecretManagerServiceClient m_client;
        private readonly string m_projectId;
        private readonly Logger m_logger;

        public GoogleSecretManagerService()
        {
            m_logger = LogManager.GetCurrentClassLogger();
            m_client = SecretManagerServiceClient.Create();
            m_projectId = GoogleComputeEngineHelper.GetCurrentProjectId();
        }

        internal async IAsyncEnumerable<string> ListSecretsAsync()
        {
            var response = m_client.ListSecretsAsync($"projects/{m_projectId}");
            await foreach (Secret item in response)
                yield return item.SecretName.SecretId;
        }

        internal async IAsyncEnumerable<string> ListSecretVersionsAsync(string secretId)
        {
            var request = new ListSecretVersionsRequest
            {
                ParentAsSecretName = SecretName.FromProjectSecret(m_projectId, secretId),
            };
            var response = m_client.ListSecretVersionsAsync(request);
            await foreach (SecretVersion item in response)
                yield return item.SecretVersionName.SecretVersionId;
        }

        internal async Task<string> GetSecretAsync(string secretId)
        {
            await foreach (var versionId in ListSecretVersionsAsync(secretId))
            {
                try
                {
                    return await AccessSecretVersion(secretId, versionId);
                }
                catch (RpcException)
                {
                    // try an older version of the secret if it exists
                }
            }

            return string.Empty;
        }

        private async Task<string> AccessSecretVersion(string secretId, string secretVersion)
        {
            m_logger.Debug("Attempting to fetch version {secretVersion} of secret {secretId}");
            var request = new AccessSecretVersionRequest
            {
                SecretVersionName = new SecretVersionName(m_projectId, secretId, secretVersion),
            };

            var response = await m_client.AccessSecretVersionAsync(request);
            m_logger.Debug("Fetched version {secretVersion} of secret {secretId}");
            return response.Payload.Data.ToStringUtf8();
        }
    }
}
