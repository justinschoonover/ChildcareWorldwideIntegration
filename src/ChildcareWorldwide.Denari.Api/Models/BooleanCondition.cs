using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class BooleanCondition
    {
        public string JoinAs { get; set; }
        public List<BooleanTerm> TermList { get; set; }
    }
}
