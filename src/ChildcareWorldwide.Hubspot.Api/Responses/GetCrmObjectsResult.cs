using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class GetCrmObjectsResult
    {
        public int? Total { get; set; }
        public List<CrmObject> Results { get; } = new List<CrmObject>();
        public PagingInfo? Paging { get; set; }
    }
}
