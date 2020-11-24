using System.Diagnostics.CodeAnalysis;
using Google.Cloud.Firestore;
using Google.Protobuf;

namespace ChildcareWorldwide.Google.Api.Models
{
    [FirestoreData]
    public record MessagingError
    {
        [FirestoreProperty]
        [NotNull]
        public string? Topic { get; init; }

        [FirestoreProperty]
        [NotNull]
        public ByteString? MessageData { get; init; }

        [FirestoreProperty]
        public string? DenariAccount { get; init; }

        [FirestoreProperty]
        public string? Email { get; init; }

        [FirestoreProperty]
        public string? Error { get; init; }
    }
}
