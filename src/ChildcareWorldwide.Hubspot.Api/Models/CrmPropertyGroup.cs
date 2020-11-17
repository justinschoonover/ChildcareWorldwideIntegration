using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record CrmPropertyGroup
    {
        [NotNull]
        public string? Name { get; init; }
        [NotNull]
        public string? Label { get; init; }
        public int? DisplayOrder { get; init; }
        public bool? Archived { get; init; }
    }
}
