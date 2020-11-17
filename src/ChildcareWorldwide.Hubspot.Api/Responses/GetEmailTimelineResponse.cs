using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Hubspot.Api.Models;

namespace ChildcareWorldwide.Hubspot.Api.Responses
{
    public sealed record GetEmailTimelineResponse
    {
        public bool HasMore { get; init; }
        [NotNull]
        public string? Offset { get; init; }
        [NotNull]
        public List<EmailTimeline>? Timeline { get; init; }
    }
}
