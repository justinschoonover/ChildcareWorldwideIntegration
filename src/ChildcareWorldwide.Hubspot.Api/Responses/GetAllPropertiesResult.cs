using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class GetAllPropertiesResult
    {
        public List<CrmProperty> Results { get; } = new List<CrmProperty>();
    }
}
