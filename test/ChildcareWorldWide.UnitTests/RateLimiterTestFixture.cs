using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ChildcareWorldwide.Hubspot.Api.Helpers;
using NUnit.Framework;

namespace ChildCareWorldWide.UnitTests
{
    public class RateLimiterTestFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestRateLimitingAsync()
        {
            // 10 requests per 1 seconds
            var ratelimiter = RateLimiter.MaxRequestsPerInterval(10, TimeSpan.FromSeconds(1));
            var stopwatch = new Stopwatch();
            var random = new Random();

            stopwatch.Start();
            for (int i = 0; i < 101; i++)
            {
                await ratelimiter.WaitForReady();

                // simulate some kind of actual network request
                await Task.Delay(random.Next(10, 50));
            }

            stopwatch.Stop();
            Assert.That(stopwatch.Elapsed >= TimeSpan.FromSeconds(10), "Expected 101 requests would take 10 seconds or more.");
        }
    }
}
