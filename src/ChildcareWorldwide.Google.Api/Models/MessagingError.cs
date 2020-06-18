using Google.Cloud.Firestore;
using Google.Protobuf;

namespace ChildcareWorldwide.Google.Api.Models
{
    [FirestoreData]
    public sealed class MessagingError
    {
        [FirestoreProperty]
        public string Topic { get; set; } = default!;

        [FirestoreProperty]
        public ByteString MessageData { get; set; } = default!;

        [FirestoreProperty]
        public string? DenariAccount { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }

        [FirestoreProperty]
        public string? Error { get; set; }
    }
}
