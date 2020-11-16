namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class CrmAssociation
    {
        public CrmAssociation(CrmObject from, CrmObject to)
        {
            From = from;
            To = to;
            Type = $"{from.ObjectType}_to_{to.ObjectType}";
        }

        public CrmObject From { get; set; } = default!;
        public CrmObject To { get; set; } = default!;
        public string Type { get; set; } = default!;
    }
}
