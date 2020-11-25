using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ChildcareWorldwide.Google.Api.PubSub
{
	public sealed class Subscriptions
	{
		public static (string Topic, string Subscription) HubspotGetDonorsForImport => GetSubscriptionForTopic(Topics.HubspotBeginImport);
		public static (string Topic, string Subscription) HubspotImportDonorAsCompany => GetSubscriptionForTopic(Topics.HubspotImportFromDonor, "-as-company");
		public static (string Topic, string Subscription) HubspotImportDonorAsContact => GetSubscriptionForTopic(Topics.HubspotImportFromDonor, "-as-contact");

		[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "C#9")]
		public static IReadOnlyList<(string Topic, string Subscription)> AllSubscriptions =>
			new List<(string, string)>(
				typeof(Subscriptions)
					.GetProperties(BindingFlags.Public | BindingFlags.Static)
					.Where(p => p.PropertyType == typeof(ValueTuple<string, string>))
					.Select(p => p.GetValue(null) as ITuple)
					.Select(t => t != null
						? (t[0]?.ToString()!, t[1]?.ToString()!)
						: default));

		private static (string, string) GetSubscriptionForTopic(string topic, string text = "") => (topic, $"{topic}{text}");
	}
}
