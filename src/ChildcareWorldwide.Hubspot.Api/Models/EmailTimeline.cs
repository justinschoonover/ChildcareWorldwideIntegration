using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record EmailTimeline
	{
		[NotNull]
		public string? Recipient { get; set; }

		[NotNull]
		public List<EmailTimelineChange>? Changes { get; init; }
	}
}
