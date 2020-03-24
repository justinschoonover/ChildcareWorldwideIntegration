using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class BooleanTerm
    {
        public string Field { get; set; }
        public string LogicalOperator { get; set; }
        public List<string> Target { get; set; }
        public string JoinAs { get; set; }
    }
}