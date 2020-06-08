using System;
using System.Collections.Generic;

namespace ChildcareWorldwide.Hubspot.Api.Models
{
    public sealed class EmailTimeline
    {
        public string Recipient { get; set; } = default!;
        public List<EmailTimelineChange> Changes { get; } = new List<EmailTimelineChange>();
    }
}
