using System;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record EmailSubscriptionStatus
    {
        public string? Id { get; init; }
        public DateTime UpdatedAt { get; init; }
        public bool Subscribed { get; init; }
        public string? OptState { get; init; }
        public string? LegalBasis { get; init; }
        public string? LegalBasisExplanation { get; init; }
    }
}
