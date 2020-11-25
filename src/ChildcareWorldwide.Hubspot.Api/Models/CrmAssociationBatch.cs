using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
	public sealed record CrmAssociationBatch
	{
		public IEnumerable<CrmAssociation>? Inputs { get; init; }
	}
}
