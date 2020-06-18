using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChildcareWorldwide.Google.Api.Helpers;
using ChildcareWorldwide.Google.Api.PubSub;
using Google.Cloud.PubSub.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NLog;

namespace ChildcareWorldwide.Google.Api
{
    public sealed class GoogleCloudPubSubService : IGoogleCloudPubSubService, IAsyncDisposable
    {
        private readonly string m_projectId;
        private readonly Logger m_logger;

        private readonly IDictionary<TopicName, PublisherClient> m_publisherClients = new Dictionary<TopicName, PublisherClient>();
        private readonly IDictionary<SubscriptionName, SubscriberClient> m_subscriberClients = new Dictionary<SubscriptionName, SubscriberClient>();

        public GoogleCloudPubSubService()
        {
            m_projectId = GoogleComputeEngineHelper.GetCurrentProjectId();
            m_logger = LogManager.GetCurrentClassLogger();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var client in m_publisherClients.Values)
            {
                try
                {
                    await client.ShutdownAsync(TimeSpan.FromSeconds(15));
                }
                catch (InvalidOperationException)
                {
                    // already shut down, nothing more to do
                }
            }

            foreach (var client in m_subscriberClients.Values)
            {
                try
                {
                    await client.StopAsync(TimeSpan.FromSeconds(15));
                }
                catch (InvalidOperationException)
                {
                    // already stopped, nothing more to do
                }
            }
        }

        public async Task InitalizeAsync(CancellationToken cancellationToken)
        {
            PublisherServiceApiClient publisherService = await PublisherServiceApiClient.CreateAsync();
            SubscriberServiceApiClient subscriberService = await SubscriberServiceApiClient.CreateAsync();

            // ensure each topic exists
            foreach (string topicId in Topics.AllTopics)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                TopicName topicName = new TopicName(m_projectId, topicId);
                try
                {
                    await publisherService.GetTopicAsync(topicName, cancellationToken);
                }
                catch (RpcException)
                {
                    Topic topic = await publisherService.CreateTopicAsync(topicName, cancellationToken);
                    m_logger.Info($"Created topic {topic.Name}");
                }

                m_publisherClients.Add(topicName, await PublisherClient.CreateAsync(topicName));
            }

            // ensure each subscription exists
            foreach (var (topicId, subscriptionId) in Subscriptions.AllSubscriptions)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                SubscriptionName subscriptionName = new SubscriptionName(m_projectId, subscriptionId);
                try
                {
                    await subscriberService.GetSubscriptionAsync(subscriptionName, cancellationToken);
                }
                catch (RpcException)
                {
                    Subscription subscription = await subscriberService.CreateSubscriptionAsync(
                        new Subscription
                        {
                            TopicAsTopicName = new TopicName(m_projectId, topicId),
                            SubscriptionName = subscriptionName,
                            AckDeadlineSeconds = 30,
                            ExpirationPolicy = new ExpirationPolicy
                            {
                                Ttl = Duration.FromTimeSpan(TimeSpan.FromDays(365)),
                            },
                        },
                        cancellationToken);
                    m_logger.Info($"Created subscription {subscription.Name}");
                }

                m_subscriberClients.Add(subscriptionName, await SubscriberClient.CreateAsync(subscriptionName));
            }
        }

        public async Task<string> PublishMessageAsync(string topic, string message)
        {
            PublisherClient client = await TryGetPublisherClient(topic);
            return await client.PublishAsync(message);
        }

        public async Task StartSubscriberAsync(string subscription, Func<PubsubMessage, CancellationToken, Task<SubscriberClient.Reply>> handlerAsync)
        {
            SubscriberClient client = await TryGetSubscriberClient(subscription);
            _ = client.StartAsync(handlerAsync);
            m_logger.Info($"Started subscriber for subscription {subscription}.");
        }

        public async Task ShutdownPublisherAsync(string topic, CancellationToken hardStopToken)
        {
            PublisherClient client = await TryGetPublisherClient(topic);
            await client.ShutdownAsync(hardStopToken);
        }

        public async Task StopSubscriberAsync(string subscription, CancellationToken hardStopToken)
        {
            SubscriberClient client = await TryGetSubscriberClient(subscription);
            await client.StopAsync(hardStopToken);
            m_logger.Info($"Stopped subscriber for subscription {subscription}.");
        }

        private async Task<PublisherClient> TryGetPublisherClient(string topic)
        {
            var topicName = new TopicName(m_projectId, topic);
            if (!m_publisherClients.TryGetValue(topicName, out PublisherClient client))
            {
                client = await PublisherClient.CreateAsync(topicName);
                m_publisherClients.Add(topicName, client);
            }

            return client;
        }

        private async Task<SubscriberClient> TryGetSubscriberClient(string subscription)
        {
            var subscriptionName = new SubscriptionName(m_projectId, subscription);
            if (!m_subscriberClients.TryGetValue(subscriptionName, out SubscriberClient client))
            {
                client = await SubscriberClient.CreateAsync(subscriptionName);
                m_subscriberClients.Add(subscriptionName, client);
            }

            return client;
        }
    }
}
