using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Google.Api.Configuration
{
    public sealed class GoogleSecretsConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new GoogleSecretsConfigurationProvider();
        }
    }
}
