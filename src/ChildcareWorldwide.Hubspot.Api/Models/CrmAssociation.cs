namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed record CrmAssociation
    {
        public CrmObject From { get; init; }
        public CrmObject To { get; init; }
        public string Type { get; init; }

        public CrmAssociation(CrmObject from, CrmObject to) => (From, To, Type) = (from, to, $"{from.ObjectType}_to_{to.ObjectType}");
    }
}