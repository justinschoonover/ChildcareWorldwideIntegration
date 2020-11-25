using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChildcareWorldWide.TestFixtures.Integration
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public sealed class HubspotServiceTestFixture : TestFixtureBase
	{
		private const int PropertyTestSampleSize = 10;

		[OneTimeSetUp]
		public void OneTimeSetup() => OneTimeSetupBase();

		[Test]
		[Category("IntegrationTest")]
		public async Task TestGetCompanyByDenariAccountIdAsync()
		{
			const string testDenariAccountId = "112196";
			var company = await HubspotService.GetCompanyByDenariAccountIdAsync(testDenariAccountId);

			Assert.NotNull(company);
			Assert.AreEqual(testDenariAccountId, company?.DenariAccountId);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestGetContactByEmailAsync()
		{
			const string testEmail = "coolrobot@hubspot.com";
			var contact = await HubspotService.GetContactByEmailAsync(testEmail);

			Assert.NotNull(contact);
			Assert.AreEqual(testEmail, contact?.Email);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestGetOptedOutEmailsAsync()
		{
			var optedOutEmails = await HubspotService.GetOptedOutEmailsAsync();
			Assert.NotNull(optedOutEmails);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestListContactPropertyGroupsAsync()
		{
			var contactPropertyGroups = await HubspotService.ListContactPropertyGroupsAsync().ToListAsync();
			Assert.NotNull(contactPropertyGroups);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestListContactPropertiesAsync()
		{
			var contactProperties = await HubspotService.ListContactPropertiesAsync().ToListAsync();
			Assert.NotNull(contactProperties);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestListCompanyPropertyGroupsAsync()
		{
			var companyPropertyGroups = await HubspotService.ListCompanyPropertyGroupsAsync().ToListAsync();
			Assert.NotNull(companyPropertyGroups);
		}

		[Test]
		[Category("IntegrationTest")]
		public async Task TestListCompanyPropertiesAsync()
		{
			var companyProperties = await HubspotService.ListCompanyPropertiesAsync().ToListAsync();
			Assert.NotNull(companyProperties);
		}

		[Test]
		[TestCaseSource(nameof(GetContactPropertyGroups))]
		[Category("IntegrationTest")]
		public async Task TestGetContactPropertyGroupAsync(string groupName)
		{
			var contactPropertyGroup = await HubspotService.GetContactPropertyGroupAsync(groupName);
			Assert.NotNull(contactPropertyGroup);
			Assert.NotNull(contactPropertyGroup?.Name);
		}

		[Test]
		[TestCaseSource(nameof(GetContactProperties))]
		[Category("IntegrationTest")]
		public async Task TestGetContactPropertyAsync(string propertyName)
		{
			var contactPropertyGroup = await HubspotService.GetContactPropertyAsync(propertyName);
			Assert.NotNull(contactPropertyGroup);
			Assert.NotNull(contactPropertyGroup?.Name);
		}

		[Test]
		[TestCaseSource(nameof(GetCompanyPropertyGroups))]
		[Category("IntegrationTest")]
		public async Task TestGetCompanyPropertyGroupAsync(string groupName)
		{
			var companyPropertyGroup = await HubspotService.GetCompanyPropertyGroupAsync(groupName);
			Assert.NotNull(companyPropertyGroup);
			Assert.NotNull(companyPropertyGroup?.Name);
		}

		[Test]
		[TestCaseSource(nameof(GetCompanyProperties))]
		[Category("IntegrationTest")]
		public async Task TestGetCompanyPropertyAsync(string propertyName)
		{
			var companyProperty = await HubspotService.GetCompanyPropertyAsync(propertyName);
			Assert.NotNull(companyProperty);
			Assert.NotNull(companyProperty?.Name);
		}

		private static IEnumerable<string> GetContactPropertyGroups() =>
			GetHubspotService().ListContactPropertyGroupsAsync().ToList().Shuffle().Take(PropertyTestSampleSize).Select(propertyGroup => propertyGroup.Name);

		private static IEnumerable<string> GetContactProperties() =>
			GetHubspotService().ListContactPropertiesAsync().ToList().Shuffle().Take(PropertyTestSampleSize).Select(propertyGroup => propertyGroup.Name);

		private static IEnumerable<string> GetCompanyPropertyGroups() =>
			GetHubspotService().ListCompanyPropertyGroupsAsync().ToList().Shuffle().Take(PropertyTestSampleSize).Select(propertyGroup => propertyGroup.Name);

		private static IEnumerable<string> GetCompanyProperties() =>
			GetHubspotService().ListCompanyPropertiesAsync().ToList().Shuffle().Take(PropertyTestSampleSize).Select(propertyGroup => propertyGroup.Name);
	}
}
