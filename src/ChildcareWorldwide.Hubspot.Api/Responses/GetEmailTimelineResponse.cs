using System.Collections.Generic;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed class GetEmailTimelineResponse
    {
        public bool HasMore { get; set; }
        public string Offset { get; set; } = default!;
        public List<EmailTimeline> Timeline { get; } = new List<EmailTimeline>();
    }
}
