using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.CustomConverters
{
    public sealed class SplitCollectionOnCrLfJsonConverter : JsonConverter<IEnumerable<string>>
    {
        private const string CrLf = "\r\n";

        public override IEnumerable<string> ReadJson(JsonReader reader, Type objectType, [AllowNull] IEnumerable<string> existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            new List<string>(((string?)reader.Value)?.Split(CrLf) ?? Array.Empty<string>());
        public override void WriteJson(JsonWriter writer, [AllowNull] IEnumerable<string> value, JsonSerializer serializer) =>
            writer.WriteValue(string.Join(CrLf, value ?? Array.Empty<string>()));
    }
}
