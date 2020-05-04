using ChildcareWorldwide.Denari.Api.Models;

namespace ChildcareWorldwide.Integration.Manager.Models
{
    public class DenariLookup : IViewModel
    {
        public string? AccountNumber { get; set; }
        public Donor? Donor { get; set; }
        public string? RawJson { get; set; }
    }
}
