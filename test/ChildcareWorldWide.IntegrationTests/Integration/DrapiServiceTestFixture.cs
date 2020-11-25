using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildcareWorldWide.TestFixtures.Integration
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public sealed class DrapiServiceTestFixture : TestFixtureBase
	{
		[OneTimeSetUp]
		public void OneTimeSetup() => OneTimeSetupBase();

		[Test]
		[Category("IntegrationTest")]
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
		[Category("IntegrationTest")]
		public async Task TestGetClassificationsForDonorAsync()
		{
			const string testDenariDonorKey = "C4113F1159";
			var classifications = await DenariService.GetClassificationsForDonorAsync(testDenariDonorKey).ToListAsync();
			Assert.True(classifications.All(c => c != null));
			Assert.True(classifications.All(c => c.DonorKey == testDenariDonorKey));
		}

		[Test]
		[Category("IntegrationTest")]
		[Explicit("Takes 30+ seconds to run.")]
		public async Task TestGetDonorsAsync()
		{
			var donors = await DenariService.GetDonorsAsync().ToListAsync();
			Assert.True(donors.All(d => d != null));
			Assert.True(donors.Any(d => d.Account == "112196"));
		}
	}
}
