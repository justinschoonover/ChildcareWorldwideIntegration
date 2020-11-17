using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public record CrmObject
    {
        [NotNull]
        public string? Id { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public bool? Archived { get; init; }
        public JObject? Properties { get; init; }

        public virtual string ObjectType { get; } = nameof(CrmObject).ToLowerInvariant();
    }
}
