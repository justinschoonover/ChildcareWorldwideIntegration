using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class DrapiFilter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
        public List<BooleanCondition> FilterTree { get; set; } = new List<BooleanCondition>();
    }
}
