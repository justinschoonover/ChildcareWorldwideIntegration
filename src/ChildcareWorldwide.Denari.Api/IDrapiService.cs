using System.Collections.Generic;
using ChildcareWorldwide.Denari.Api.Models;

namespace ChildcareWorldwide.Denari.Api
{
    public interface IDrapiService
    {
        IAsyncEnumerable<Donor> GetDonorsAsync();
    }
}
