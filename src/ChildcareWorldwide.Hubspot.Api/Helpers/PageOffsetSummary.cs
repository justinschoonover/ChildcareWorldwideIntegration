using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Helpers
{
	internal sealed class PageOffsetSummary<T>
		where T : class
	{
		public PageOffsetSummary(ICollection<T> results, string offset, bool hasMore)
		{
			HasMore = hasMore;
			Offset = offset;
			Results = results;
		}

		public bool HasMore { get; }
		public string Offset { get; }
		public ICollection<T> Results { get; }
	}
}
