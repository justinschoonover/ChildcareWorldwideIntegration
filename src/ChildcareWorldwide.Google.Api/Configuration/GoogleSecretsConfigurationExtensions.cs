using Microsoft.Extensions.Configuration;

namespace ChildcareWorldwide.Google.Api.Configuration
{
    public static class GoogleSecretsConfigurationExtensions
    {
        public static IConfigurationBuilder AddGoogleSecretsConfiguration(this IConfigurationBuilder builder)
        {
            return builder.Add(new GoogleSecretsConfigurationSource());
        }
    }
}
