using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class GetAllPropertyGroupsResult
    {
        public List<CrmPropertyGroup> Results { get; } = new List<CrmPropertyGroup>();
    }
}
