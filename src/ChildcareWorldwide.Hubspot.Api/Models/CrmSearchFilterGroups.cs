using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record CrmSearchFilterGroups
	{
		[NotNull]
		public List<CrmSearchFilter>? Filters { get; init; }
	}
}
