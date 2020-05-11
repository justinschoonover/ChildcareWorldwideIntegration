using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildcareWorldWide.IntegrationTests
{
    public sealed class HubspotServiceTestFixture : TestFixtureBase
    {
        [OneTimeSetUp]
        public void OneTimeSetup() => OneTimeSetupBase();

        [Test]
        public async Task TestGetCompanyByDenariAccountIdAsync()
        {
            const string testDenariAccountId = "112196";
            var company = await HubspotService.GetCompanyByDenariAccountIdAsync(testDenariAccountId);

            Assert.NotNull(company);
            Assert.AreEqual(testDenariAccountId, company!.DenariAccountId);
        }
    }
}
