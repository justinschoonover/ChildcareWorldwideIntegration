using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ChildcareWorldwide.Hubspot.Api.Attributes;
using ChildcareWorldwide.Hubspot.Api.CustomConverters;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Mappers
{
	public static class DomainModelMapper
	{
		public static T? MapDomainModel<T>(CrmObject? obj)
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

			foreach (var property in obj.Properties.Properties())
				TrySetValue(mappedObj, property.Name, property.Value.ToString());

			return mappedObj;
		}

		public static T FilterEnumerationPropertyValues<T>(this T domainModel, CrmProperty property)
		{
			var validPropertyValues = property.Options?.Select(o => o.Label) ?? Enumerable.Empty<string>();
			var currentValues = GetDomainModelProperties(domainModel)
				.Where(p => (p.JsonPropertyAttribute?.PropertyName ?? p.PropertyInfo.Name.ToLowerInvariant()) == property.Name)
				.Select(p => p.PropertyInfo.GetValue(domainModel)?.ToString())
				.SelectMany(s => s?.Split(';') ?? Array.Empty<string>());

			TrySetValue(domainModel, property.Name, string.Join(';', currentValues.Intersect(validPropertyValues)));
			return domainModel;
		}

		public static string GetPropertiesForCreate<T>(T domainModel)
		{
			var properties = new JObject();
			foreach (var (property, jsonProperty) in GetDomainModelProperties(domainModel))
			{
				if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
					continue;

				if (property.GetValue(domainModel) != null)
					properties.Add(new JProperty($"{jsonProperty?.PropertyName ?? property.Name}".ToLowerInvariant(), property.GetValue(domainModel)));
			}

			return JsonConvert.SerializeObject(new JObject(new JProperty("properties", properties)), Formatting.Indented, new DateTimeJsonConverter(), new DecimalJsonConverter());
		}

		[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "C#9")]
		public static bool TryGetPropertiesForUpdate<T>(T updated, T existing, out string result)
		{
			var properties = new JObject();
			var existingProperties = GetDomainModelProperties(existing).ToDictionary(p => p.PropertyInfo.Name, p => p);
			foreach (var (property, jsonProperty) in GetDomainModelProperties(updated))
			{
				if (property.GetCustomAttribute<JsonIgnoreAttribute>() != null)
					continue;

				if (!property.IsValueNullOrEmpty(updated) && !property.GetValue(updated)!.Equals(existingProperties[property.Name].PropertyInfo.GetValue(existing)))
					properties.Add(new JProperty($"{jsonProperty?.PropertyName ?? property.Name}".ToLowerInvariant(), property.GetValue(updated)));
			}

			result = JsonConvert.SerializeObject(new JObject(new JProperty("properties", properties)), Formatting.Indented, new DateTimeJsonConverter(), new DecimalJsonConverter());
			return properties.Count > 0;
		}

		public static List<string> GetPropertyNames<T>(T domainModel) =>
			GetDomainModelProperties(domainModel)
				.Select(p => p.JsonPropertyAttribute?.PropertyName ?? p.PropertyInfo.Name.ToLowerInvariant())
				.ToList();

		public static List<string> GetPropertiesToFilterBy<T>(T domainModel) =>
			GetDomainModelProperties(domainModel)
				.Where(p => p.PropertyInfo.GetCustomAttribute<FilterToAvailableHubspotPropertyValuesAttribute>() != null)
				.Select(p => p.JsonPropertyAttribute?.PropertyName ?? p.PropertyInfo.Name.ToLowerInvariant())
				.ToList();

		[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "C#9")]
		private static bool IsValueNullOrEmpty<T>([NotNullWhen(false)] this PropertyInfo property, T instance)
		{
			if (property.GetValue(instance) == null)
				return true;

			return (Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType) == typeof(string) && property.GetValue(instance)!.Equals(string.Empty);
		}

		private static void TrySetValue<T>(T domainModel, string name, string value)
		{
			try
			{
				// actual property names
				var propertyInfo = domainModel?.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
				if (propertyInfo == null)
				{
					var matchingProperties = domainModel?
						.GetType()
						.GetProperties(BindingFlags.Public | BindingFlags.Instance)
						.Select(p => (Property: p, JsonProperty: p.GetCustomAttribute<JsonPropertyAttribute>()))
						.Where(pair => pair.JsonProperty != null && pair.JsonProperty.PropertyName == name)
						.Select(filteredPair => filteredPair.Property)
						.ToList();

					if (matchingProperties == null || matchingProperties.Count > 1)
						throw new InvalidOperationException($"Multiple {typeof(T)} properties map to the same Hubspot field.");

					propertyInfo = matchingProperties.SingleOrDefault();
				}

				// property isn't present in our model
				if (propertyInfo == null)
					return;

				var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
				object? convertedValue = Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
				propertyInfo.SetValue(domainModel, convertedValue, null);
			}
			catch (Exception)
			{
				return;
			}
		}

		private static IEnumerable<(PropertyInfo PropertyInfo, JsonPropertyAttribute? JsonPropertyAttribute)> GetDomainModelProperties<T>(T domainModel)
		{
			if (domainModel == null)
				throw new ArgumentNullException(nameof(domainModel));

			return domainModel
				.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Select(p => (Property: p, JsonProperty: p.GetCustomAttribute<JsonPropertyAttribute>()));
		}
	}
}
