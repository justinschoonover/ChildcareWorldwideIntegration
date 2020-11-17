using System;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.DomainModels
{
    // CCW domain model for Hubspot Contacts
    public record Contact : CrmObject
    {
        [JsonIgnore]
        public override string ObjectType => nameof(Contact).ToLowerInvariant();

        [NotNull]
        public string? Email { get; init; }

        [JsonProperty("denari_account")]
        public string? DenariAccountId { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        [JsonProperty("spouse_secondary_contact")]
        public string? SecondaryContact { get; init; }

        [JsonProperty("contact_name")]
        public string? ContactName { get; init; }

        [JsonProperty("salutation")]
        public string? DenariSalutation { get; init; }

        [JsonProperty("esalutation")]
        public string? ESalutation { get; init; }

        [JsonProperty("record_type")]
        public string? DonorType { get; init; }

        public string? Organization { get; init; }

        [JsonProperty("address")]
        public string? StreetAddress { get; init; }

        [JsonProperty("street_address_2")]
        public string? StreetAddress2 { get; init; }

        public string? City { get; init; }

        public string? State { get; init; }

        public string? Zip { get; init; }

        public string? Country { get; init; }

        public string? Phone { get; init; }

        [JsonProperty("home_phone_2_denari")]
        public string? HomePhone2 { get; init; }

        [JsonProperty("mobilephone")]
        public string? MobilePhone { get; init; }

        [JsonProperty("other_phone")]
        public string? OtherPhone { get; init; }

        [JsonProperty("work_phone")]
        public string? WorkPhone { get; init; }

        [JsonProperty("initial_source_code_denari")]
        public string? DonorAppealCode { get; init; }

        [JsonProperty("initial_source_denari")]
        public string? DonorAppealDesc { get; init; }

        [JsonProperty("source_classifications")]
        public string? SourceClassifications { get; init; }

        [JsonProperty("communication_classifications")]
        public string? CommunicationClassifications { get; init; }

        [JsonProperty("miscellaneous_classifications")]
        public string? MiscellaneousClassifications { get; init; }

        [JsonProperty("relationship_classifications")]
        public string? RelationshipClassifications { get; init; }

        [JsonProperty("deliverable_denari")]
        public bool Deliverable { get; init; }

        [JsonProperty("denari_intro_date")]
        public DateTime? DenariIntroDate { get; init; }

        [JsonProperty("first_gift_date")]
        public DateTime? FirstGiftDate { get; init; }

        [JsonProperty("first_gift_amount")]
        public decimal? FirstGiftAmount { get; init; }

        [JsonProperty("last_gift_date")]
        public DateTime? LastGiftDate { get; init; }

        [JsonProperty("last_gift_amount")]
        public decimal? LastGiftAmount { get; init; }

        [JsonProperty("big_gift")]
        public decimal? BigGiftAmount { get; init; }

        [JsonProperty("small_gift")]
        public decimal? SmallGiftAmount { get; init; }

        [JsonProperty("total_gifts")]
        public decimal? TotalGiftsAmount { get; init; }

        [JsonProperty("gift_count")]
        public int? GiftCount { get; init; }

        [JsonProperty("average_gift")]
        public decimal? AverageGiftAmount { get; init; }

        [JsonProperty("gifts_ytd")]
        public decimal? GiftsYtd { get; init; }

        [JsonProperty("gifts_last_year")]
        public decimal? GiftsLastYear { get; init; }

        [JsonProperty("gifts_2_years_ago")]
        public decimal? Gifts2YearsAgo { get; init; }

        [JsonProperty("gifts_3_years_ago")]
        public decimal? Gifts3YearsAgo { get; init; }

        [JsonProperty("gifts_4_years_ago")]
        public decimal? Gifts4YearsAgo { get; init; }

        public bool International { get; init; }

        public string? Website { get; init; }

        public string? Gender { get; init; }

        [JsonProperty("date_of_birth")]
        public string? DateOfBirth { get; init; }

        [JsonProperty("date_of_birth_spouse_secondary_")]
        public DateTime? DateOfBirthSpouse { get; init; }

        [JsonProperty("jurisdiction_denari")]
        public string? Jurisdiction { get; init; }

        [JsonProperty("denari_notes")]
        public string? Notes { get; init; }

        [JsonProperty("alternate_address")]
        public string? AlternateAddress { get; init; }

        [JsonProperty("olc_username")]
        public string? OlcUsername { get; init; }

        [JsonProperty("sponsored_child_name_1")]
        public string? SponsoredChildName1 { get; init; }

        [JsonProperty("sponsored_child_name_2")]
        public string? SponsoredChildName2 { get; init; }

        [JsonProperty("sponsored_child_name_3")]
        public string? SponsoredChildName3 { get; init; }

        [JsonProperty("sponsored_child_name_4")]
        public string? SponsoredChildName4 { get; init; }
    }
}
