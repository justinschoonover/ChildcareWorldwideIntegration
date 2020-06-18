using System;
using ChildcareWorldwide.Denari.Api.Models;
using Google.Cloud.PubSub.V1;

namespace ChildcareWorldwide.Integration.Subscriber.Models
{
    public sealed class ProcessMessageResult
    {
        public ProcessMessageResult()
        {
        }

        public ProcessMessageResult(SubscriberClient.Reply response)
        {
            Response = response;
        }

        public SubscriberClient.Reply Response { get; set; } = default!;
        public Donor? Donor { get; set; }
        public string? Email { get; set; }
        public Exception? Exception { get; set; }
    }
}
