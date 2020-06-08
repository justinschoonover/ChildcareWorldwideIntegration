using System;
using System.Globalization;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Hubspot.Api.Mappers;
using NUnit.Framework;

namespace ChildcareWorldWide.UnitTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public sealed class HubspotApiMapperTestFixture
    {
        [Test]
        public void TestGetPropertiesForCreate()
        {
            var company = new Company
            {
                DenariAccountId = "12345",
                DenariIntroDate = DateTime.Parse("1980-10-02", DateTimeFormatInfo.InvariantInfo),
                BigGiftAmount = 12345m,
            };

            string properties = DomainModelMapper.GetPropertiesForCreate(company);
            Assert.NotNull(properties);

            // strings should be quoted
            Assert.True(properties.Contains($"\"account_id\": \"{company.DenariAccountId}\"", StringComparison.InvariantCulture));

            // dates should be milliseconds since unix epoch
            var dateAsUnixOffset = new DateTimeOffset((DateTime)company.DenariIntroDate).ToUnixTimeMilliseconds().ToString(DateTimeFormatInfo.InvariantInfo);
            Assert.True(properties.Contains($"\"denari_intro_date\": \"{dateAsUnixOffset}\"", StringComparison.InvariantCulture));

            // numbers should be quoted
            var stringGiftAmount = $"{company.BigGiftAmount:N}".Replace(",", string.Empty, StringComparison.InvariantCulture);
            Assert.True(properties.Contains($"\"big_gift_amount\": \"{stringGiftAmount}\"", StringComparison.InvariantCulture));
        }

        [Test]
        public void TestGetPropertiesForUpdate()
        {
            var existing = new Company
            {
                DenariAccountId = "12345",
            };

            var updated = new Company
            {
                DenariAccountId = "54321",
            };

            string properties = DomainModelMapper.GetPropertiesForUpdate(updated, existing);
            Assert.NotNull(properties);

            // new property should be present, not old one
            Assert.True(properties.Contains($"\"account_id\": \"{updated.DenariAccountId}\"", StringComparison.InvariantCulture));
            Assert.False(properties.Contains($"\"account_id\": \"{existing.DenariAccountId}\"", StringComparison.InvariantCulture));
        }
    }
}
