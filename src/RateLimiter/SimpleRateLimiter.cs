using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RateLimiter
{
	public sealed class SimpleRateLimiter : IDisposable
	{
		private static readonly TimeSpan FudgeFactor = TimeSpan.FromMilliseconds(100);

		private readonly int m_maxPerInterval;
		private readonly TimeSpan m_interval;
		private readonly List<DateTime> m_previousRequestsInInterval;
		private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);

		private SimpleRateLimiter(int maxPerInterval, TimeSpan interval)
		{
			m_maxPerInterval = maxPerInterval;
			m_interval = interval;
			m_previousRequestsInInterval = new List<DateTime>();
		}

		public static SimpleRateLimiter MaxRequestsPerInterval(int maxRequests, TimeSpan interval) => new SimpleRateLimiter(maxRequests, interval);

		public async Task WaitForReady(CancellationToken cancellationToken = default)
		{
			await m_semaphore.WaitAsync(cancellationToken);
			var now = DateTime.Now;

			// remove old timestamps we no longer need to track
			m_previousRequestsInInterval.RemoveAll(r => now - r >= m_interval);

			int count = m_previousRequestsInInterval.Count;
			m_previousRequestsInInterval.Add(now);

			if (count < m_maxPerInterval)
			{
				m_semaphore.Release();
				return;
			}

			var timeToWait = m_previousRequestsInInterval.Min().Add(m_interval) - now + FudgeFactor;
			await Task.Delay(timeToWait, cancellationToken);

			m_semaphore.Release();
		}

		public void Dispose() => m_semaphore.Dispose();
	}
}
