using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ChildcareWorldwide.Denari.Api.Models;
using ChildcareWorldwide.Hubspot.Api.DomainModels;
using ChildcareWorldwide.Integration.Subscriber.Helpers;

namespace ChildcareWorldwide.Integration.Subscriber.Mappers
{
	public static class IntegrationMapper
	{
		public static Company MapDonorToCompany(Donor donor) =>
			new Company
			{
				DenariAccountId = donor.Account,
				Name = donor.Type switch
				{
					"Organization" => donor.Organization,
					"Business" => donor.Organization,
					"Church" => donor.Organization,
					"Foundation" => donor.Organization,
					"Individual" => donor.ContactName,
					_ => donor.ContactName,
				},
				FirstName = donor.FirstName,
				LastName = donor.LastName,
				SecondaryContact = donor.Spouse,
				DenariContactName = donor.ContactName,
				DenariSalutation = donor.Salutation,
				CompanyType = donor.Type,
				StreetAddress = donor.Street,
				StreetAddress2 = donor.Street2,
				City = donor.City,
				State = donor.State,
				Zip = donor.Zip,
				Country = donor.Country,
				Phone = donor.Phone,
				MobilePhone = donor.CellPhone,
				WorkPhone = donor.WorkPhone,
				DenariIntroDate = donor.AddedDate.SpecifyUtc(),
				FirstGiftDate = donor.FirstGiftDate.SpecifyUtc(),
				FirstGiftAmount = donor.FirstGiftAmount,
				LastGiftDate = donor.LastGiftDate.SpecifyUtc(),
				LastGiftAmount = donor.LastGiftAmount,
				BigGiftAmount = donor.BigGift,
				TotalGiftsAmount = donor.TotalGifts,
				GiftCount = donor.GiftCount,
				AverageGiftAmount = donor.AverageGift,
				GiftsYtd = donor.GiftsYtd,
				GiftsLastYear = donor.GiftsLastYear,
				Gifts2YearsAgo = donor.Gifts2YearsAgo,
				Gifts3YearsAgo = donor.Gifts3YearsAgo,
				Gifts4YearsAgo = donor.Gifts4YearsAgo,
				CommunicationClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				MiscellaneousClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				RelationshipClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				SourceClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
			};

		public static Contact MapDonorToContact(Donor donor, string email) =>
			new Contact
			{
				DenariAccountId = donor.Account,
				Email = email,
				FirstName = email.Equals(donor.Email, StringComparison.InvariantCultureIgnoreCase)
					? donor.FirstName
					: donor.Spouse,
				LastName = donor.LastName,
				SecondaryContact = email.Equals(donor.Email, StringComparison.InvariantCultureIgnoreCase)
					? donor.Spouse
					: donor.FirstName,
				ContactName = donor.ContactName?.NullIfEmpty() ?? donor.Organization,
				DenariSalutation = donor.Salutation,
				DonorType = donor.Type,
				StreetAddress = donor.Street,
				StreetAddress2 = donor.Street2,
				City = donor.City,
				State = donor.State,
				Zip = donor.Zip,
				Country = donor.Country,
				Phone = donor.Phone,
				MobilePhone = donor.CellPhone,
				WorkPhone = donor.WorkPhone,
				DonorAppealCode = donor.AppealCode,
				DonorAppealDesc = donor.Appeal,
				DenariIntroDate = donor.AddedDate.SpecifyUtc(),
				FirstGiftDate = donor.FirstGiftDate.SpecifyUtc(),
				FirstGiftAmount = donor.FirstGiftAmount,
				LastGiftDate = donor.LastGiftDate.SpecifyUtc(),
				LastGiftAmount = donor.LastGiftAmount,
				BigGiftAmount = donor.BigGift,
				TotalGiftsAmount = donor.TotalGifts,
				GiftCount = donor.GiftCount,
				AverageGiftAmount = donor.AverageGift,
				GiftsYtd = donor.GiftsYtd,
				GiftsLastYear = donor.GiftsLastYear,
				Gifts2YearsAgo = donor.Gifts2YearsAgo,
				Gifts3YearsAgo = donor.Gifts3YearsAgo,
				Gifts4YearsAgo = donor.Gifts4YearsAgo,
				International = IsForeignCountry(donor.Country),
				Gender = donor.Gender,
				DateOfBirth = donor.Dob.SpecifyUtc()?.ToString("d", DateTimeFormatInfo.InvariantInfo),
				DateOfBirthSpouse = donor.DobSpouse.SpecifyUtc(),
				Notes = string.Join("\n", donor.Notes ?? Enumerable.Empty<string>()),
				CommunicationClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				MiscellaneousClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				RelationshipClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
				SourceClassifications = string.Join(";", donor.Classifications?.Select(c => c.ClassificationDescription) ?? Enumerable.Empty<string>()),
			};

		private static bool IsForeignCountry(string? country)
		{
			if (country.IsNullOrEmpty())
				return false;

			var knownUnitedStatesDenariVariants = new List<string>
			{
				"United States",
				"United States of America",
				"US",
				"USA",
				"UNITED STATES",
				"usa",
				"U. S. A.",
				"U.S.",
				"United State",
				"U.S.A.",
			};

			return !knownUnitedStatesDenariVariants.Contains(country);
		}

		private static DateTime? SpecifyUtc(this DateTime? dateTime) =>
			dateTime != null
				? (DateTime?)DateTime.SpecifyKind((DateTime)dateTime, DateTimeKind.Utc)
				: null;
	}
}
