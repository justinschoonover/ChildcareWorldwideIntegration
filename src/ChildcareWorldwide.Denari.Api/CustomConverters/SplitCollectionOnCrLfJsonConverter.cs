using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.CustomConverters
{
    public sealed class SplitCollectionOnCrLfJsonConverter : JsonConverter<IEnumerable<string>>
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override IEnumerable<string> ReadJson(JsonReader reader, Type objectType, IEnumerable<string> existingValue, bool hasExistingValue, JsonSerializer serializer) => new List<string>(((string?)reader.Value)?.Split("\r\n"));
        public override void WriteJson(JsonWriter writer, IEnumerable<string> value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
