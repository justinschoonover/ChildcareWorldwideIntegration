using System;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.DomainModels
{
	// CCW domain model for Hubspot Companies
	public sealed record Company : CrmObject
	{
		[JsonIgnore]
		public override string ObjectType => nameof(Company).ToLowerInvariant();

		public string? Name { get; init; }

		[JsonProperty("account_id")]
		[NotNull]
		public string? DenariAccountId { get; init; }

		[JsonProperty("first_name")]
		public string? FirstName { get; init; }

		[JsonProperty("last_name")]
		public string? LastName { get; init; }

		[JsonProperty("secondary_contact")]
		public string? SecondaryContact { get; init; }

		[JsonProperty("contact_name_denari")]
		public string? DenariContactName { get; init; }

		[JsonProperty("salutation_denari")]
		public string? DenariSalutation { get; init; }

		[JsonProperty("salutation")]
		public string? ESalutation { get; init; }

		public string? Organization { get; init; }

		[JsonProperty("company_type")]
		public string? CompanyType { get; init; }

		[JsonProperty("address")]
		public string? StreetAddress { get; init; }

		[JsonProperty("address2")]
		public string? StreetAddress2 { get; init; }

		public string? City { get; init; }

		public string? State { get; init; }

		public string? Zip { get; init; }

		public string? Country { get; init; }

		public string? Phone { get; init; }

		[JsonProperty("home_phone_2_denari")]
		public string? HomePhone2 { get; init; }

		[JsonProperty("mobile_phone_number")]
		public string? MobilePhone { get; init; }

		[JsonProperty("other_phone")]
		public string? OtherPhone { get; init; }

		[JsonProperty("work_phone")]
		public string? WorkPhone { get; init; }

		[JsonProperty("denari_intro_date")]
		public DateTime? DenariIntroDate { get; init; }

		[JsonProperty("initial_source")]
		public string? DenariInitialSource { get; init; }

		public string? Recency { get; init; }

		public int? Frequency { get; init; }

		public string? Amount { get; init; }

		[JsonProperty("first_gift_date")]
		public DateTime? FirstGiftDate { get; init; }

		[JsonProperty("first_gift_amount")]
		public decimal? FirstGiftAmount { get; init; }

		[JsonProperty("last_gift_date")]
		public DateTime? LastGiftDate { get; init; }

		[JsonProperty("last_gift_amount")]
		public decimal? LastGiftAmount { get; init; }

		[JsonProperty("big_gift_amount")]
		public decimal? BigGiftAmount { get; init; }

		[JsonProperty("small_gift_amount")]
		public decimal? SmallGiftAmount { get; init; }

		[JsonProperty("total_gifts_amount")]
		public decimal? TotalGiftsAmount { get; init; }

		[JsonProperty("gift_count")]
		public int? GiftCount { get; init; }

		[JsonProperty("average_gift_amount")]
		public decimal? AverageGiftAmount { get; init; }

		[JsonProperty("gifts_ytd")]
		public decimal? GiftsYtd { get; init; }

		[JsonProperty("gifts_last_year")]
		public decimal? GiftsLastYear { get; init; }

		[JsonProperty("gifts_made_2_years_ago")]
		public decimal? Gifts2YearsAgo { get; init; }

		[JsonProperty("gifts_3_years_ago")]
		public decimal? Gifts3YearsAgo { get; init; }

		[JsonProperty("gifts_4_years_ago")]
		public decimal? Gifts4YearsAgo { get; init; }

		[JsonProperty("source_classifications")]
		public string? SourceClassifications { get; init; }

		[JsonProperty("communication_classifications")]
		public string? CommunicationClassifications { get; init; }

		[JsonProperty("miscellaneous_classifications")]
		public string? MiscellaneousClassifications { get; init; }

		[JsonProperty("relationship_classifications")]
		public string? RelationshipClassifications { get; init; }
	}
}
