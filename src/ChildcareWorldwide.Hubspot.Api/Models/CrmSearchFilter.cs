using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record CrmSearchFilter
	{
		[NotNull]
		public string? PropertyName { get; init; }

		[NotNull]
		public string? Operator { get; init; }

		public string? Value { get; init; }
	}
}
