using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Mappers
{
    public static class DomainModelMapper
    {
        public static T? MapDomainModel<T>(CrmObject obj)
            where T : CrmObject, new()
        {
            if (obj == null)
                return null;

            var mappedObj = new T
            {
                Id = obj.Id,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = obj.UpdatedAt,
            };

            if (obj.Properties == null)
                return mappedObj;

            foreach (JProperty property in obj.Properties.Properties())
            {
                TrySetValue(mappedObj, property.Name, property.Value.ToString());
            }

            return mappedObj;
        }

        public static string GetPropertiesForCreate<T>(T domainModel)
        {
            JObject properties = new JObject();
            foreach (var (property, jsonProperty) in GetDomainModelProperties(domainModel))
            {
                if (property.GetValue(domainModel) != null)
                    properties.Add(new JProperty($"{jsonProperty?.PropertyName ?? property.Name}".ToLower(CultureInfo.InvariantCulture), property.GetValue(domainModel)));
            }

            return JsonConvert.SerializeObject(new JObject(new JProperty("properties", properties)), Formatting.Indented, new DateTimeJsonConverter(), new DecimalJsonConverter());
        }

        public static bool GetPropertiesForUpdate<T>(T updated, T existing, out string result)
        {
            JObject properties = new JObject();
            var existingProperties = GetDomainModelProperties(existing).ToDictionary(p => p.PropertyInfo.Name, p => p);
            foreach (var (property, jsonProperty) in GetDomainModelProperties(updated))
            {
                if (!property.IsValueNullOrEmpty(updated) && !property.GetValue(updated).Equals(existingProperties[property.Name].PropertyInfo.GetValue(existing)))
                    properties.Add(new JProperty($"{jsonProperty?.PropertyName ?? property.Name}".ToLower(CultureInfo.InvariantCulture), property.GetValue(updated)));
            }

            result = new JObject(new JProperty("properties", properties)).ToString();
            return properties.Count > 0;
        }

        public static List<string> GetPropertyNames<T>(T domainModel)
        {
            return GetDomainModelProperties(domainModel).Select(p => p.JsonPropertyAttribute?.PropertyName ?? p.PropertyInfo.Name).ToList();
        }

        private static bool IsValueNullOrEmpty<T>([NotNullWhen(false)] this PropertyInfo property, T instance)
        {
            if (property.GetValue(instance) == null)
                return true;

            if ((Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType) == typeof(string) && property.GetValue(instance).Equals(string.Empty))
                return true;

            return false;
        }

        private static bool TrySetValue<T>(T domainModel, string name, string value)
        {
            try
            {
                // actual property names
                var propertyInfo = domainModel?.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    propertyInfo = domainModel?
                        .GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => (Property: p, JsonProperty: p.GetCustomAttribute<JsonPropertyAttribute>()))
                        .Where(t => t.JsonProperty != null && t.JsonProperty.PropertyName == name)
                        .Select(t => t.Property)
                        .SingleOrDefault();
                }

                // property isn't present in our model
                if (propertyInfo == null)
                    return false;

                Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var convertedValue = value == null ? default : Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
                propertyInfo.SetValue(domainModel, convertedValue, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static IEnumerable<(PropertyInfo PropertyInfo, JsonPropertyAttribute JsonPropertyAttribute)> GetDomainModelProperties<T>(T domainModel)
        {
            if (domainModel == null)
                throw new ArgumentNullException(nameof(domainModel));

            return domainModel.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => (Property: p, JsonProperty: p.GetCustomAttribute<JsonPropertyAttribute>()));
        }

        private class DateTimeJsonConverter : JsonConverter<DateTime>
        {
            public override bool CanRead => false;

            public override DateTime ReadJson(JsonReader reader, Type objectType, [AllowNull] DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, [AllowNull] DateTime value, JsonSerializer serializer)
            {
                writer.WriteValue(new DateTimeOffset(value).ToUnixTimeMilliseconds().ToString(DateTimeFormatInfo.InvariantInfo));
            }
        }

        private class DecimalJsonConverter : JsonConverter<decimal>
        {
            public override bool CanRead => false;

            public override decimal ReadJson(JsonReader reader, Type objectType, [AllowNull] decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, [AllowNull] decimal value, JsonSerializer serializer)
            {
                NumberFormatInfo noGroupSeparator = new CultureInfo(string.Empty, false).NumberFormat;
                noGroupSeparator.NumberGroupSeparator = string.Empty;
                writer.WriteValue(value.ToString("N", noGroupSeparator));
            }
        }
    }
}
