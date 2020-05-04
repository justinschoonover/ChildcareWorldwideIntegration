using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class BooleanTerm
    {
        public string Field { get; set; } = default!;
        public string LogicalOperator { get; set; } = default!;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
        public List<string> Target { get; set; } = new List<string>();
        public string JoinAs { get; set; } = default!;
    }
}