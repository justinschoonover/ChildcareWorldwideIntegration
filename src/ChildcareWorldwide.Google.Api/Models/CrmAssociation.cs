using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace ChildcareWorldwide.Google.Api.Models
{
    [FirestoreData]
    public record CrmAssociation
    {
        [FirestoreProperty]
        public string? CompanyId { get; init; }

        [FirestoreProperty]
        public IEnumerable<string>? ContactIds { get; init; }
    }
}
