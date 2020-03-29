using System;
using System.Collections.Generic;
using ChildcareWorldwide.Denari.Api.CustomConverters;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.Models
{
    public sealed class Donor
    {
        public Donor(string donorKey)
        {
            DonorKey = donorKey;
        }

        [JsonProperty("Donor_Account")]
        public string Account { get; set; } = default!;

        [JsonProperty("Donor_FirstName")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? FirstName { get; set; }

        [JsonProperty("Donor_LastName")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? LastName { get; set; }

        [JsonProperty("Donor_Spouse")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Spouse { get; set; }

        [JsonProperty("Donor_Contactname")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? ContactName { get; set; }

        [JsonProperty("Donor_Salutation")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Salutation { get; set; }

        [JsonProperty("Donor_Organization")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Organization { get; set; }

        [JsonProperty("Donor_Street")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Street { get; set; }

        [JsonProperty("Donor_Street2")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Street2 { get; set; }

        [JsonProperty("Donor_City")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? City { get; set; }

        [JsonProperty("Donor_State")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? State { get; set; }

        [JsonProperty("Donor_Zip")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Zip { get; set; }

        [JsonProperty("Donor_Country")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Country { get; set; }

        [JsonProperty("Donor_JobTitle")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? JobTitle { get; set; }

        [JsonProperty("Donor_Email")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Email { get; set; }

        [JsonProperty("Donor_Email2")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Email2 { get; set; }

        [JsonProperty("Donor_Phone")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Phone { get; set; }

        [JsonProperty("Donor_WorkPhone")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? WorkPhone { get; set; }

        [JsonProperty("Donor_CellPhone")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? CellPhone { get; set; }

        [JsonProperty("Donor_Notes")]
        [JsonConverter(typeof(SplitCollectionOnCrLfJsonConverter))]
        public IEnumerable<string> Notes { get; set; } = default!;

        [JsonProperty("Donor_DOB")]
        public DateTime? Dob { get; set; }

        [JsonProperty("Donor_DOBSpouse")]
        public DateTime? DobSpouse { get; set; }

        [JsonProperty("Donor_Gender")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Gender { get; set; }

        [JsonProperty("Donor_Username")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Username { get; set; }

        [JsonProperty("Donor_GiftCount")]
        public int GiftCount { get; set; }

        [JsonProperty("Donor_FirstGiftDate")]
        public DateTime? FirstGiftDate { get; set; }

        [JsonProperty("Donor_FirstGiftAmount")]
        public float FirstGiftAmount { get; set; }

        [JsonProperty("Donor_LastGiftDate")]
        public DateTime? LastGiftDate { get; set; }

        [JsonProperty("Donor_LastGiftAmount")]
        public float LastGiftAmount { get; set; }

        [JsonProperty("Donor_BigGift")]
        public float BigGift { get; set; }

        [JsonProperty("Donor_AverageGift")]
        public float AverageGift { get; set; }

        [JsonProperty("Donor_TotalGifts")]
        public float TotalGifts { get; set; }

        [JsonProperty("Donor_GiftsYTD")]
        public float GiftsYtd { get; set; }

        [JsonProperty("Donor_GiftsLastYear")]
        public float GiftsLastYear { get; set; }

        [JsonProperty("Donor_Gifts2YearsAgo")]
        public float Gifts2YearsAgo { get; set; }

        [JsonProperty("Donor_Gifts3YearsAgo")]
        public float Gifts3YearsAgo { get; set; }

        [JsonProperty("Donor_Gifts4YearsAgo")]
        public float Gifts4YearsAgo { get; set; }

        [JsonProperty("Donor_Added_Datetime")]
        public DateTime? AddedDateTime { get; set; }

        [JsonProperty("Donor_Added_Date")]
        public DateTime? AddedDate { get; set; }

        [JsonProperty("Donor_Type")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Type { get; set; }

        [JsonProperty("Donor_Jurisdiction_Code")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? JurisdictionCode { get; set; }

        [JsonProperty("Donor_Jurisdiction")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Jurisdiction { get; set; }

        [JsonProperty("Donor_Mailcode")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Mailcode { get; set; }

        [JsonProperty("Donor_Appeal_Code")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? AppealCode { get; set; }

        [JsonProperty("Donor_Appeal")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string? Appeal { get; set; }

        [JsonProperty("Donor_JUR_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string JurisdictionKey { get; set; } = default!;

        [JsonProperty("Donor_TYPE_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string DonorTypeKey { get; set; } = default!;

        [JsonProperty("Donor_NAME1_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string DonorKey { get; set; } = default!;

        [JsonProperty("Donor_SOL_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string AppealKey { get; set; } = default!;

        [JsonProperty("COMP_REF")]
        [JsonConverter(typeof(TrimStringJsonConverter))]
        public string CompanyKey { get; set; } = default!;
    }
}
