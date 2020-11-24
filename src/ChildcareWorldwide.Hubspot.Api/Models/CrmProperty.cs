using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record CrmProperty
    {
        [NotNull]
        public string? Name { get; init; }
        [NotNull]
        public string? Label { get; init; }
        [NotNull]
        public string? Type { get; init; }
        [NotNull]
        public string? FieldType { get; init; }
        [NotNull]
        public string? GroupName { get; init; }
        public string? Description { get; init; }
        public List<CrmPropertyOptions>? Options { get; init; }
        public int? DisplayOrder { get; init; }
        public bool? HasUniqueValue { get; init; }
        public bool? Hidden { get; init; }
        public DateTime? CreatedAt { get; init; }
    }
}
