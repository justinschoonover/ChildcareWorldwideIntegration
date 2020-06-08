using System;
using ChildcareWorldwide.Hubspot.Api.Models;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Hubspot.Api.DomainModels
{
    // CCW domain model for Hubspot Companies
    public sealed class Company : CrmObject
    {
        public string? Name { get; set; }

        [JsonProperty("account_id")]
        public string DenariAccountId { get; set; } = default!;

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("secondary_contact")]
        public string? SecondaryContact { get; set; }

        [JsonProperty("contact_name_denari")]
        public string? DenariContactName { get; set; }

        [JsonProperty("salutation_denari")]
        public string? DenariSalutation { get; set; }

        [JsonProperty("salutation")]
        public string? ESalutation { get; set; }

        public string? Organization { get; set; }

        [JsonProperty("company_type")]
        public string? CompanyType { get; set; }

        [JsonProperty("address")]
        public string? StreetAddress { get; set; }

        [JsonProperty("address2")]
        public string? StreetAddress2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        [JsonProperty("home_phone_2_denari")]
        public string? HomePhone2 { get; set; }

        [JsonProperty("mobile_phone_number")]
        public string? MobilePhone { get; set; }

        [JsonProperty("other_phone")]
        public string? OtherPhone { get; set; }

        [JsonProperty("work_phone")]
        public string? WorkPhone { get; set; }

        [JsonProperty("denari_intro_date")]
        public DateTime? DenariIntroDate { get; set; }

        [JsonProperty("initial_source")]
        public string? DenariInitialSource { get; set; }

        public string? Recency { get; set; }

        public int? Frequency { get; set; }

        public string? Amount { get; set; }

        [JsonProperty("first_gift_date")]
        public DateTime? FirstGiftDate { get; set; }

        [JsonProperty("first_gift_amount")]
        public decimal? FirstGiftAmount { get; set; }

        [JsonProperty("last_gift_date")]
        public DateTime? LastGiftDate { get; set; }

        [JsonProperty("last_gift_amount")]
        public decimal? LastGiftAmount { get; set; }

        [JsonProperty("big_gift_amount")]
        public decimal? BigGiftAmount { get; set; }

        [JsonProperty("small_gift_amount")]
        public decimal? SmallGiftAmount { get; set; }

        [JsonProperty("total_gifts_amount")]
        public decimal? TotalGiftsAmount { get; set; }

        [JsonProperty("gift_count")]
        public int? GiftCount { get; set; }

        [JsonProperty("average_gift_amount")]
        public decimal? AverageGiftAmount { get; set; }

        [JsonProperty("gifts_ytd")]
        public decimal? GiftsYtd { get; set; }

        [JsonProperty("gifts_last_year")]
        public decimal? GiftsLastYear { get; set; }

        [JsonProperty("gifts_made_2_years_ago")]
        public decimal? Gifts2YearsAgo { get; set; }

        [JsonProperty("gifts_3_years_ago")]
        public decimal? Gifts3YearsAgo { get; set; }

        [JsonProperty("gifts_4_years_ago")]
        public decimal? Gifts4YearsAgo { get; set; }

        [JsonProperty("source_classifications")]
        public string? SourceClassifications { get; set; }

        [JsonProperty("communication_classifications")]
        public string? CommunicationClassifications { get; set; }

        [JsonProperty("miscellaneous_classifications")]
        public string? MiscellaneousClassifications { get; set; }

        [JsonProperty("miscellaneous_classifications")]
        public string? RelationshipClassifications { get; set; }
    }
}
