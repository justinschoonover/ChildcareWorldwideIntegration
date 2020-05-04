namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmPropertyOptions
    {
        public string Label { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool Hidden { get; set; } = default!;
    }
}