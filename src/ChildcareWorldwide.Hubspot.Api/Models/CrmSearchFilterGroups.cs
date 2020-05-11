using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmSearchFilterGroups
    {
        public List<CrmSearchFilter> Filters { get; } = new List<CrmSearchFilter>();
    }
}
