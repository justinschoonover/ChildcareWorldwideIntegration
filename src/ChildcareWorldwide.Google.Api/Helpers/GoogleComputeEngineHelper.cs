using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChildcareWorldwide.Google.Api.Helpers
{
	public static class GoogleComputeEngineHelper
	{
		public static string GetCurrentProjectId() => GetCurrentProjectIdAsync().GetAwaiter().GetResult();

		public static async Task<string> GetCurrentProjectIdAsync()
		{
			try
			{
				const string path = "http://metadata.google.internal/computeMetadata/v1/project/project-id";
				using var client = new HttpClient();
				client.DefaultRequestHeaders.Add("Metadata-Flavor", "Google");
				return await client.GetStringAsync(path);
			}
			catch (HttpRequestException)
			{
				// look for local developer credentials
				string? credentialsFile = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
				if (File.Exists(credentialsFile))
				{
					var credentials = JsonConvert.DeserializeObject<GoogleServiceAccountKey>(await File.ReadAllTextAsync(credentialsFile!));
					return credentials.ProjectId;
				}

				return string.Empty;
			}
		}

		private sealed record GoogleServiceAccountKey
		{
			[JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
			[NotNull]
			public string? ProjectId { get; init; }
		}
	}
}
