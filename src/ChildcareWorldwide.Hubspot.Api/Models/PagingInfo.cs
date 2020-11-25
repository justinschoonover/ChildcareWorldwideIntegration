using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record PagingInfo
	{
		[NotNull]
		public PagingNext? Next { get; init; }
	}
}
