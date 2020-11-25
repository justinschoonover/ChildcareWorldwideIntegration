using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Denari.Api.CustomConverters;
using Newtonsoft.Json;

namespace ChildcareWorldwide.Denari.Api.Models
{
	public record Donor
	{
		public Donor(string donorKey) => DonorKey = donorKey;

		[JsonProperty("Donor_Account")]
		public string Account { get; init; } = default!;

		[JsonProperty("Donor_FirstName")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? FirstName { get; init; }

		[JsonProperty("Donor_LastName")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? LastName { get; init; }

		[JsonProperty("Donor_Spouse")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Spouse { get; init; }

		[JsonProperty("Donor_Contactname")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? ContactName { get; init; }

		[JsonProperty("Donor_Salutation")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Salutation { get; init; }

		[JsonProperty("Donor_Organization")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Organization { get; init; }

		[JsonProperty("Donor_Street")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Street { get; init; }

		[JsonProperty("Donor_Street2")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Street2 { get; init; }

		[JsonProperty("Donor_City")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? City { get; init; }

		[JsonProperty("Donor_State")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? State { get; init; }

		[JsonProperty("Donor_Zip")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Zip { get; init; }

		[JsonProperty("Donor_Country")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Country { get; init; }

		[JsonProperty("Donor_JobTitle")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? JobTitle { get; init; }

		[JsonProperty("Donor_Email")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Email { get; init; }

		[JsonProperty("Donor_Email2")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Email2 { get; init; }

		[JsonProperty("Donor_Phone")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Phone { get; init; }

		[JsonProperty("Donor_WorkPhone")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? WorkPhone { get; init; }

		[JsonProperty("Donor_CellPhone")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? CellPhone { get; init; }

		[JsonProperty("Donor_Notes")]
		[JsonConverter(typeof(SplitCollectionOnCrLfJsonConverter))]
		public IEnumerable<string>? Notes { get; init; }

		[JsonProperty("Donor_DOB")]
		public DateTime? Dob { get; init; }

		[JsonProperty("Donor_DOBSpouse")]
		public DateTime? DobSpouse { get; init; }

		[JsonProperty("Donor_Gender")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Gender { get; init; }

		[JsonProperty("Donor_Username")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Username { get; init; }

		[JsonProperty("Donor_GiftCount")]
		public int GiftCount { get; init; }

		[JsonProperty("Donor_FirstGiftDate")]
		public DateTime? FirstGiftDate { get; init; }

		[JsonProperty("Donor_FirstGiftAmount")]
		public decimal FirstGiftAmount { get; init; }

		[JsonProperty("Donor_LastGiftDate")]
		public DateTime? LastGiftDate { get; init; }

		[JsonProperty("Donor_LastGiftAmount")]
		public decimal LastGiftAmount { get; init; }

		[JsonProperty("Donor_BigGift")]
		public decimal BigGift { get; init; }

		[JsonProperty("Donor_AverageGift")]
		public decimal AverageGift { get; init; }

		[JsonProperty("Donor_TotalGifts")]
		public decimal TotalGifts { get; init; }

		[JsonProperty("Donor_GiftsYTD")]
		public decimal GiftsYtd { get; init; }

		[JsonProperty("Donor_GiftsLastYear")]
		public decimal GiftsLastYear { get; init; }

		[JsonProperty("Donor_Gifts2YearsAgo")]
		public decimal Gifts2YearsAgo { get; init; }

		[JsonProperty("Donor_Gifts3YearsAgo")]
		public decimal Gifts3YearsAgo { get; init; }

		[JsonProperty("Donor_Gifts4YearsAgo")]
		public decimal Gifts4YearsAgo { get; init; }

		[JsonProperty("Donor_Added_Datetime")]
		public DateTime? AddedDateTime { get; init; }

		[JsonProperty("Donor_Added_Date")]
		public DateTime? AddedDate { get; init; }

		[JsonProperty("Donor_Type")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Type { get; init; }

		[JsonProperty("Donor_Jurisdiction_Code")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? JurisdictionCode { get; init; }

		[JsonProperty("Donor_Jurisdiction")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Jurisdiction { get; init; }

		[JsonProperty("Donor_Mailcode")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Mailcode { get; init; }

		[JsonProperty("Donor_Appeal_Code")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? AppealCode { get; init; }

		[JsonProperty("Donor_Appeal")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		public string? Appeal { get; init; }

		[JsonProperty("Donor_JUR_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		[NotNull]
		public string? JurisdictionKey { get; init; }

		[JsonProperty("Donor_TYPE_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		[NotNull]
		public string? DonorTypeKey { get; init; }

		[JsonProperty("Donor_NAME1_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		[NotNull]
		public string? DonorKey { get; init; }

		[JsonProperty("Donor_SOL_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		[NotNull]
		public string? AppealKey { get; init; }

		[JsonProperty("COMP_REF")]
		[JsonConverter(typeof(TrimStringJsonConverter))]
		[NotNull]
		public string? CompanyKey { get; init; }

		public IEnumerable<DonorClassification>? Classifications { get; init; }
	}
}
