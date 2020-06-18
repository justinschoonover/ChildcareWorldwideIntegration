using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api.Models;

namespace ChildcareWorldwide.Denari.Api
{
    public interface IDrapiService
    {
        Task<(Donor? donor, string? rawJson)> GetDonorByAccountAsync(string accountNumber, CancellationToken cancellationToken = default);
        IAsyncEnumerable<Donor> GetDonorsAsync(CancellationToken cancellationToken = default);
        IAsyncEnumerable<DonorClassification> GetClassificationsForDonorAsync(string donorKey, CancellationToken cancellationToken = default);
    }
}
