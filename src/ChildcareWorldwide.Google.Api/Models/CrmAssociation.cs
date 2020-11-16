using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace ChildcareWorldwide.Google.Api.Models
{
    [FirestoreData]
    public sealed class CrmAssociation
    {
        [FirestoreProperty]
        public string? CompanyId { get; set; }

        [FirestoreProperty]
        public IEnumerable<string>? ContactIds { get; set; }
    }
}
