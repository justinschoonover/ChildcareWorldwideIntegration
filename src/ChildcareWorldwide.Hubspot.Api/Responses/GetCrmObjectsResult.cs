using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
	public sealed record GetCrmObjectsResult
	{
		public int? Total { get; init; }

		[NotNull]
		public List<CrmObject>? Results { get; init; }

		public PagingInfo? Paging { get; init; }
	}
}
