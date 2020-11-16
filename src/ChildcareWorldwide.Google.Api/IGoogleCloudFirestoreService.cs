using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Models;

namespace ChildcareWorldwide.Google.Api
{
    public interface IGoogleCloudFirestoreService
    {
        public Task AddMessagingErrorAsync(MessagingError messagingError, CancellationToken cancellationToken);
        public Task AddDenariCompanyAssociationAsync(string denariAccount, string companyId, CancellationToken cancellationToken);
        public Task AddDenariContactAssociationsAsync(string denariAccount, IEnumerable<string> contactIds, CancellationToken cancellationToken);
        public Task<CrmAssociation?> GetHubspotAssociationsForDonorAsync(string denariAccount, CancellationToken cancellationToken);
    }
}
