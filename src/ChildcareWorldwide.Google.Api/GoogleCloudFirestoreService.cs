using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Helpers;
using ChildcareWorldwide.Google.Api.Models;
using Google.Cloud.Firestore;
using NLog;

namespace ChildcareWorldwide.Google.Api
{
    public sealed class GoogleCloudFirestoreService : IGoogleCloudFirestoreService
    {
        private readonly FirestoreDb m_db;
        private readonly Logger m_logger;

        public GoogleCloudFirestoreService()
        {
            m_db = FirestoreDb.Create(GoogleComputeEngineHelper.GetCurrentProjectId());
            m_logger = LogManager.GetCurrentClassLogger();
        }

        public async Task AddMessagingError(MessagingError messagingError, CancellationToken cancellationToken)
        {
            CollectionReference collection = m_db.Collection($"pubsub-errors");
            await collection.AddAsync(messagingError, cancellationToken: cancellationToken);
        }
    }
}
