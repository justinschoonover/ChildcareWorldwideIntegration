using System.Collections.Generic;
using System.Threading.Tasks;
using ChildcareWorldwide.Denari.Api.Models;

namespace ChildcareWorldwide.Denari.Api
{
    public interface IDrapiService
    {
        Task<Donor?> GetDonorByAccountAsync(string accountNumber);
        IAsyncEnumerable<Donor> GetDonorsAsync();
    }
}
