using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class GetEmailSubscriptionStatusResult
    {
        public bool Subscribed { get; set; }
        public bool MarkedAsSpam { get; set; }
        public bool UnsubscribeFromPortal { get; set; }
        public string? PortalId { get; set; }
        public bool Bounced { get; set; }
        public string? Email { get; set; }
        public List<EmailSubscriptionStatus> SubscriptionStatuses { get; } = new List<EmailSubscriptionStatus>();
        public string? Status { get; set; }
    }
}
