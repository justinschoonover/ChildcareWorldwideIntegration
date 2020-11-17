using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record PagingNext
    {
        [NotNull]
        public string? After { get; init; }
        [NotNull]
        public string? Link { get; init; }
    }
}
