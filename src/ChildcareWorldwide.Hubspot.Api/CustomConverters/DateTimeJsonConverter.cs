using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.CustomConverters
{
	public sealed class DateTimeJsonConverter : JsonConverter<DateTime>
	{
		public override bool CanRead => false;

		public override DateTime ReadJson(JsonReader reader, Type objectType, [AllowNull] DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();

		public override void WriteJson(JsonWriter writer, [AllowNull] DateTime value, JsonSerializer serializer) => writer.WriteValue(new DateTimeOffset(value).ToUnixTimeMilliseconds().ToString(DateTimeFormatInfo.InvariantInfo));
	}
}
