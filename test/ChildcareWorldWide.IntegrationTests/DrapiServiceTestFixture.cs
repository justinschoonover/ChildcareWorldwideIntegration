using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildcareWorldWide.IntegrationTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public sealed class DrapiServiceTestFixture : TestFixtureBase
    {
        [OneTimeSetUp]
        public void OneTimeSetup() => OneTimeSetupBase();

        [Test]
        public async Task TestGetDonorByAccountAsync()
        {
            const string testDenariAccountId = "112196";
            var (donor, json) = await DenariService.GetDonorByAccountAsync(testDenariAccountId);
            Assert.NotNull(donor);
            Assert.AreEqual(testDenariAccountId, donor?.Account);
            Assert.NotNull(json);
            Assert.True(json?.Contains(testDenariAccountId, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public async Task TestGetDonorsAsync()
        {
            var donors = await DenariService.GetDonorsAsync().ToListAsync();
            Assert.True(donors.Any(d => d.Account == "112196"));
            foreach (var donor in donors)
            {
                Assert.NotNull(donor);
            }

            foreach (var country in donors.Select(d => d.Country).Distinct())
                Console.WriteLine(country);
        }
    }
}
