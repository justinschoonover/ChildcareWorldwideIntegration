namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmPropertyGroup
    {
        public string Name { get; set; } = default!;
        public string Label { get; set; } = default!;
        public int? DisplayOrder { get; set; }
        public bool? Archived { get; set; }
    }
}
