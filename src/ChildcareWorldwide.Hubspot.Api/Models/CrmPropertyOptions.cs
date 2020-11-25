using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record CrmPropertyOptions
	{
		[NotNull]
		public string? Label { get; init; }

		[NotNull]
		public string? Value { get; init; }

		public string? Description { get; init; }
		public int? DisplayOrder { get; init; }

		[NotNull]
		public bool? Hidden { get; init; }
	}
}
