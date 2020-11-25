using ChildcareWorldwide.Denari.Api.CustomConverters;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.Models
{
	public record DonorClassification
	{
		[JsonProperty("Classification_Code")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? ClassificationCode { get; init; }

		[JsonProperty("Classification_Description")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? ClassificationDescription { get; init; }

		[JsonProperty("Classification_NAME1_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? DonorKey { get; init; }

		[JsonProperty("Classification_FCAT_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? ClassificationKey { get; init; }

		[JsonProperty("COMP_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? CompanyKey { get; init; }
	}
}
