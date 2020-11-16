using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public class CrmObject
    {
        public string Id { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Archived { get; set; }
        [SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Breaks deserialization")]
        public JObject? Properties { get; set; }

        public virtual string ObjectType { get; } = nameof(CrmObject).ToLowerInvariant();
    }
}
