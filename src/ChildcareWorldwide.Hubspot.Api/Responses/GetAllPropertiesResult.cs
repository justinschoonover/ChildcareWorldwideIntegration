using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
	public sealed record GetAllPropertiesResult
	{
		[NotNull]
		public List<CrmProperty>? Results { get; init; }
	}
}
