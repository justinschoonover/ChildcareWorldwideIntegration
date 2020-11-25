using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Denari.Api.Models;

namespace ChildcareWorldwide.Denari.Api.Responses
{
	public sealed record DonorList<T>
	{
		public string? ResultCode { get; init; }
		public string? Message { get; init; }
		public string? ErrorNumber { get; init; }
		public string? ErrorMessage { get; init; }
		public string? ListName { get; init; }
		public int PageSize { get; init; }
		public int PageCount { get; init; }
		public int CurrentPage { get; init; }
		public string? Order { get; init; }
		public List<string>? Fields { get; init; }
		public DrapiFilter? Filter { get; init; }

		[NotNull]
		public List<T>? Data { get; init; }
	}
}
