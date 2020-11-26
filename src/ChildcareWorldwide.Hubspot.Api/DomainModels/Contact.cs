using System;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Attributes;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.DomainModels
{
	// CCW domain model for Hubspot Contacts
	public sealed class Contact : CrmObject
	{
		[JsonIgnore]
		public override string ObjectType => nameof(Contact).ToLowerInvariant();

		[NotNull]
		public string? Email { get; set; }

		[JsonProperty("denari_account")]
		public string? DenariAccountId { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		[JsonProperty("spouse_secondary_contact")]
		public string? SecondaryContact { get; set; }

		[JsonProperty("contact_name")]
		public string? ContactName { get; set; }

		[JsonProperty("salutation")]
		public string? DenariSalutation { get; set; }

		[JsonProperty("esalutation")]
		public string? ESalutation { get; set; }

		[JsonProperty("record_type")]
		public string? DonorType { get; set; }

		public string? Organization { get; set; }

		[JsonProperty("address")]
		public string? StreetAddress { get; set; }

		[JsonProperty("street_address_2")]
		public string? StreetAddress2 { get; set; }

		public string? City { get; set; }

		public string? State { get; set; }

		public string? Zip { get; set; }

		public string? Country { get; set; }

		public string? Phone { get; set; }

		[JsonProperty("home_phone_2_denari")]
		public string? HomePhone2 { get; set; }

		[JsonProperty("mobilephone")]
		public string? MobilePhone { get; set; }

		[JsonProperty("other_phone")]
		public string? OtherPhone { get; set; }

		[JsonProperty("work_phone")]
		public string? WorkPhone { get; set; }

		[JsonProperty("initial_source_code_denari")]
		public string? DonorAppealCode { get; set; }

		[JsonProperty("initial_source_denari")]
		public string? DonorAppealDesc { get; set; }

		[JsonProperty("source_classification_cloned_")]
		[FilterToAvailableHubspotPropertyValues]
		public string? SourceClassifications { get; set; }

		[JsonProperty("communication_classifications")]
		[FilterToAvailableHubspotPropertyValues]
		public string? CommunicationClassifications { get; set; }

		[JsonProperty("miscellaneous_classifications")]
		[FilterToAvailableHubspotPropertyValues]
		public string? MiscellaneousClassifications { get; set; }

		[JsonProperty("relationship_classifications")]
		[FilterToAvailableHubspotPropertyValues]
		public string? RelationshipClassifications { get; set; }

		[JsonProperty("deliverable_denari")]
		public bool Deliverable { get; set; }

		[JsonProperty("denari_intro_date")]
		public DateTime? DenariIntroDate { get; set; }

		[JsonProperty("first_gift_date")]
		public DateTime? FirstGiftDate { get; set; }

		[JsonProperty("first_gift_amount")]
		public decimal? FirstGiftAmount { get; set; }

		[JsonProperty("last_gift_date")]
		public DateTime? LastGiftDate { get; set; }

		[JsonProperty("last_gift_amount")]
		public decimal? LastGiftAmount { get; set; }

		[JsonProperty("big_gift")]
		public decimal? BigGiftAmount { get; set; }

		[JsonProperty("small_gift")]
		public decimal? SmallGiftAmount { get; set; }

		[JsonProperty("total_gifts")]
		public decimal? TotalGiftsAmount { get; set; }

		[JsonProperty("gift_count")]
		public int? GiftCount { get; set; }

		[JsonProperty("average_gift")]
		public decimal? AverageGiftAmount { get; set; }

		[JsonProperty("gifts_ytd")]
		public decimal? GiftsYtd { get; set; }

		[JsonProperty("gifts_last_year")]
		public decimal? GiftsLastYear { get; set; }

		[JsonProperty("gifts_2_years_ago")]
		public decimal? Gifts2YearsAgo { get; set; }

		[JsonProperty("gifts_3_years_ago")]
		public decimal? Gifts3YearsAgo { get; set; }

		[JsonProperty("gifts_4_years_ago")]
		public decimal? Gifts4YearsAgo { get; set; }

		public bool International { get; set; }

		public string? Website { get; set; }

		public string? Gender { get; set; }

		[JsonProperty("date_of_birth")]
		public string? DateOfBirth { get; set; }

		[JsonProperty("date_of_birth_spouse_secondary_")]
		public DateTime? DateOfBirthSpouse { get; set; }

		[JsonProperty("jurisdiction_denari")]
		public string? Jurisdiction { get; set; }

		[JsonProperty("denari_notes")]
		public string? Notes { get; set; }

		[JsonProperty("alternate_address")]
		public string? AlternateAddress { get; set; }

		[JsonProperty("olc_username")]
		public string? OlcUsername { get; set; }

		[JsonProperty("sponsored_child_name_1")]
		public string? SponsoredChildName1 { get; set; }

		[JsonProperty("sponsored_child_name_2")]
		public string? SponsoredChildName2 { get; set; }

		[JsonProperty("sponsored_child_name_3")]
		public string? SponsoredChildName3 { get; set; }

		[JsonProperty("sponsored_child_name_4")]
		public string? SponsoredChildName4 { get; set; }
	}
}
