using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record CrmSearchOptions
    {
        [NotNull]
        public List<CrmSearchFilterGroups>? FilterGroups { get; init; }
        public List<string>? Sorts { get; init; }
        public string? Query { get; init; }

        public List<string>? Properties { get; init; }
        public int? Limit { get; init; }
        public int? After { get; init; }
    }
}
