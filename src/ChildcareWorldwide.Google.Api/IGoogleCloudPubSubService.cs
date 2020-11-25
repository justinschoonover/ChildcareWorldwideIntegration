using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;

namespace ChildcareWorldwide.Google.Api
{
	public interface IGoogleCloudPubSubService
	{
		Task InitalizeAsync(CancellationToken cancellationToken);
		Task<string> PublishMessageAsync(string topic, string message);
		Task ShutdownPublisherAsync(string topic, CancellationToken hardStopToken);
		Task StartSubscriberAsync(string subscription, Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> handleAsync);
		Task StopSubscriberAsync(string subscription, CancellationToken hardStopToken);
	}
}
