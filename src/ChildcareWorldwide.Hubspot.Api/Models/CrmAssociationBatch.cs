using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmAssociationBatch
    {
        public IEnumerable<CrmAssociation> Inputs { get; set; } = new List<CrmAssociation>();
    }
}
