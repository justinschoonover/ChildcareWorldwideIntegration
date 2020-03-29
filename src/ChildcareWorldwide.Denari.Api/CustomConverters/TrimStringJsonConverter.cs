using System;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.CustomConverters
{
    public sealed class TrimStringJsonConverter : JsonConverter<string?>
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer) => ((string?)reader.Value)?.Trim();
        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
