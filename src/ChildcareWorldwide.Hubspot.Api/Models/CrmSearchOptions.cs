using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmSearchOptions
    {
        public List<CrmSearchFilterGroups> FilterGroups { get; } = new List<CrmSearchFilterGroups>();
        public List<string> Sorts { get; } = new List<string>();
        public string? Query { get; set; }
        public List<string> Properties { get; } = new List<string>();
        public int? Limit { get; set; }
        public int? After { get; set; }
    }
}
