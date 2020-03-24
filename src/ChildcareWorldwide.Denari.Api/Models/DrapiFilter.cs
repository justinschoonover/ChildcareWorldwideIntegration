using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class DrapiFilter
    {
        public List<BooleanCondition> FilterTree { get; set; }
    }
}
