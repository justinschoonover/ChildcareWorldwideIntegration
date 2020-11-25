using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChildcareWorldwide.Hubspot.Api.Helpers
{
	internal static class ApiPagingUtility
	{
		public static async Task<IReadOnlyList<T>> IterateAsync<T>(Func<string, Task<PageOffsetSummary<T>>> call)
			where T : class
		{
			if (call == null)
				throw new ArgumentNullException(nameof(call));

			var results = new List<T>();
			var page = new PageOffsetSummary<T>(results, string.Empty, true);
			while (page.HasMore)
			{
				page = await call(page.Offset);
				results.AddRange(page.Results);
			}

			return results;
		}
	}
}
