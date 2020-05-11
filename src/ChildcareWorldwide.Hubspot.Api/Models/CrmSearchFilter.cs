namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmSearchFilter
    {
        public string PropertyName { get; set; } = default!;
        public string Operator { get; set; } = default!;
        public string? Value { get; set; }
    }
}
