using System;
using System.Collections.Generic;
using System.Text;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class BooleanCondition
    {
        public string JoinAs { get; set; }
        public List<BooleanTerm> TermList { get; set; }
    }
}
