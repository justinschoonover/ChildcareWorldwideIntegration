using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public record BooleanCondition
    {
        public string? JoinAs { get; init; }
        public List<BooleanTerm>? TermList { get; init; }
    }
}
