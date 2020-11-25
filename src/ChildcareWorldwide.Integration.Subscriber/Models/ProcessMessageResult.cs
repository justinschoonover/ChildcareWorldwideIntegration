using System;
using ChildcareWorldwide.Denari.Api.Models;
using Google.Cloud.PubSub.V1;

namespace ChildcareWorldwide.Integration.Subscriber.Models
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "C#9")]
	public sealed record ProcessMessageResult(SubscriberClient.Reply Response)
	{
		public Donor? Donor { get; init; }
		public string? Email { get; init; }
		public Exception? Exception { get; init; }
	}
}
