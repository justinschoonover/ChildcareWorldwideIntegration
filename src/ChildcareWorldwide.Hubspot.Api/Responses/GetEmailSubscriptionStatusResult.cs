using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
	public sealed record GetEmailSubscriptionStatusResult
	{
		public bool Subscribed { get; init; }
		public bool MarkedAsSpam { get; init; }
		public bool UnsubscribeFromPortal { get; init; }
		public string? PortalId { get; init; }
		public bool Bounced { get; init; }
		public string? Email { get; init; }
		public List<EmailSubscriptionStatus>? SubscriptionStatuses { get; init; }
		public string? Status { get; init; }
	}
}
