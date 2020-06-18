using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmSearchOptions
    {
        public List<CrmSearchFilterGroups> FilterGroups { get; } = new List<CrmSearchFilterGroups>();
        public List<string> Sorts { get; } = new List<string>();
        public string? Query { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
        public List<string> Properties { get; set; } = new List<string>();
        public int? Limit { get; set; }
        public int? After { get; set; }
    }
}
