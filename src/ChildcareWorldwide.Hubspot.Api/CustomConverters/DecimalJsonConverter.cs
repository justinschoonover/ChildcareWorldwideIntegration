using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.CustomConverters
{
	public sealed class DecimalJsonConverter : JsonConverter<decimal>
	{
		public override bool CanRead => false;

		public override decimal ReadJson(JsonReader reader, Type objectType, [AllowNull] decimal existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();

		public override void WriteJson(JsonWriter writer, [AllowNull] decimal value, JsonSerializer serializer)
		{
			var noGroupSeparator = new CultureInfo(string.Empty, false).NumberFormat;
			noGroupSeparator.NumberGroupSeparator = string.Empty;
			writer.WriteValue(value.ToString("N", noGroupSeparator));
		}
	}
}
