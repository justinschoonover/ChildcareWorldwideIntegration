using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Breaks deserialization")]
    public sealed class CrmObject
    {
        public string Id { get; set; } = default!;
        public JObject? Properties { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CrmAssociations? Associations { get; set; }
        public bool Archived { get; set; }
    }
}
