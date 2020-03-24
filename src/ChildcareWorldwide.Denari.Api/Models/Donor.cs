using System;
using System.Collections.Generic;
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
        public string Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Spouse { get; set; }
        public string Contactname { get; set; }
        public string Salutation { get; set; }
        public string Organization { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string Phone { get; set; }
        public string WorkPhone { get; set; }
        public string CellPhone { get; set; }
        public IEnumerable<string> Notes { get; set; }
        public DateTime Dob { get; set; }
        public DateTime DobSpouse { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public int GiftCount { get; set; }
        public DateTime FirstGiftDate { get; set; }
        public float FirstGiftAmount { get; set; }
        public DateTime LastGiftDate { get; set; }
        public float LastGiftAmount { get; set; }
        public float BigGift { get; set; }
        public float AverageGift { get; set; }
        public float TotalGifts { get; set; }
        public float GiftsYtd { get; set; }
        public float GiftsLastYear { get; set; }
        public float Gifts2YearsAgo { get; set; }
        public float Gifts3YearsAgo { get; set; }
        public float Gifts4YearsAgo { get; set; }
        public DateTime AddedDateTime { get; set; }
        public DateTime AddedDate { get; set; }
        public string Type { get; set; }
        public string JurisdictionCode { get; set; }
        public string Jurisdiction { get; set; }
        public string Mailcode { get; set; }
        public string AppealCode { get; set; }
        public string Appeal { get; set; }
        public string JurisdictionKey { get; set; }
        public string DonorTypeKey { get; set; }

        [JsonProperty("Donor_NAME1_REF")]
        public string DonorKey { get; set; }
        public string AppealKey { get; set; }
        public string CompanyKey { get; set; }
    }
}
