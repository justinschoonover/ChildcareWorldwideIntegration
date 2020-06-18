using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Models;

namespace ChildcareWorldwide.Google.Api
{
    public interface IGoogleCloudFirestoreService
    {
        public Task AddMessagingError(MessagingError messagingError, CancellationToken cancellationToken);
    }
}
