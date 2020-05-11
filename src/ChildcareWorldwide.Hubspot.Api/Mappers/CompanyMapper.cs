using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Mappers
{
    public static class CompanyMapper
    {
        public static Company? MapCompany(CrmObject obj)
        {
            if (obj == null)
                return null;

            var company = new Company
            {
                Id = obj.Id,
                CreatedDate = obj.CreatedAt,
                UpdatedDate = obj.UpdatedAt,
            };

            if (obj.Properties == null)
                return company;

            foreach (JProperty property in obj.Properties.Properties())
            {
                TrySetValue(company, property.Name, property.Value.ToString());
            }

            return company;
        }

        private static bool TrySetValue(Company company, string name, string value)
        {
            try
            {
                // actual property names
                var propertyInfo = company.GetType().GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    propertyInfo = company
                        .GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(p => (Property: p, JsonProperty: p.GetCustomAttribute<JsonPropertyAttribute>()))
                        .Where(t => t.JsonProperty != null && t.JsonProperty.PropertyName == name)
                        .Select(t => t.Property)
                        .Single();
                }

                // property isn't present in our model
                if (propertyInfo == null)
                    return false;

                var convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
                propertyInfo.SetValue(company, convertedValue, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
