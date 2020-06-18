using ChildcareWorldwide.Denari.Api.CustomConverters;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class DonorClassification
    {
        [JsonProperty("Classification_Code")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string ClassificationCode { get; set; } = default!;

        [JsonProperty("Classification_Description")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string ClassificationDescription { get; set; } = default!;

        [JsonProperty("Classification_NAME1_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string DonorKey { get; set; } = default!;

        [JsonProperty("Classification_FCAT_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string ClassificationKey { get; set; } = default!;

        [JsonProperty("COMP_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string CompanyKey { get; set; } = default!;
    }
}
