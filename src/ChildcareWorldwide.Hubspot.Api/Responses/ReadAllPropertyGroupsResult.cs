using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class ReadAllPropertyGroupsResult
    {
        public List<CrmPropertyGroup> Results { get; } = new List<CrmPropertyGroup>();
    }
}
