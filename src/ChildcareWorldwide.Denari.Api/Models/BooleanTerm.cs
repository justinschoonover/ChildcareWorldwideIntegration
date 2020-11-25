using System.Collections.Generic;

namespace ChildcareWorldwide.Denari.Api.Models
{
	public record BooleanTerm
	{
		public string? Field { get; init; }
		public string? LogicalOperator { get; init; }
		public List<string>? Target { get; init; }
		public string? JoinAs { get; init; }
	}
}
