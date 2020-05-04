using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class BooleanCondition
    {
        public string JoinAs { get; set; } = default!;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
        public List<BooleanTerm> TermList { get; set; } = new List<BooleanTerm>();
    }
}
