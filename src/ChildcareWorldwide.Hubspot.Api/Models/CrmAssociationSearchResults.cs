using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmAssociationSearchResults
    {
        public List<CrmAssociation> Results { get; } = new List<CrmAssociation>();
    }
}
