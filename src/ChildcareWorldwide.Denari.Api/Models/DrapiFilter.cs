using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public record DrapiFilter
    {
        public List<BooleanCondition>? FilterTree { get; init; }
    }
}
