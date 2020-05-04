using System;
using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmProperty
    {
        public string Name { get; set; } = default!;
        public string Label { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string FieldType { get; set; } = default!;
        public string GroupName { get; set; } = default!;
        public string? Description { get; set; }
        public List<CrmPropertyOptions> Options { get; } = new List<CrmPropertyOptions>();
        public int? DisplayOrder { get; set; }
        public bool? HasUniqueValue { get; set; }
        public bool? Hidden { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
