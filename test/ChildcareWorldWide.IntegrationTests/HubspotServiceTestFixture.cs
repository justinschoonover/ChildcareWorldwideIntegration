using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildcareWorldWide.IntegrationTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
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
            Assert.AreEqual(testDenariAccountId, company?.DenariAccountId);
        }

        [Test]
        public async Task TestGetContactByEmailAsync()
        {
            const string testEmail = "coolrobot@hubspot.com";
            var contact = await HubspotService.GetContactByEmailAsync(testEmail);

            Assert.NotNull(contact);
            Assert.AreEqual(testEmail, contact?.Email);
        }

        [Test]
        public async Task TestGetOptedOutEmailsAsync()
        {
            var optedOutEmails = await HubspotService.GetOptedOutEmailsAsync();

            Assert.NotNull(optedOutEmails);
        }
    }
}
