using System;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class EmailSubscriptionStatus
    {
        public string? Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Subscribed { get; set; }
        public string? OptState { get; set; }
        public string? LegalBasis { get; set; }
        public string? LegalBasisExplanation { get; set; }
    }
}
