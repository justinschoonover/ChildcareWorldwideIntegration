using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed record GetAllPropertyGroupsResult
    {
        [NotNull]
        public List<CrmPropertyGroup>? Results { get; init; }
    }
}
