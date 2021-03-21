﻿using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using WebHost.Infrastructure.Infrastructure.MessageBrokers;

namespace WebHost.Infrastructure.Infrastructure.MassTransitConsumers
{
    public class InMemoryBrokerPublisher : BrokerPublisherBase
    {
        private readonly IPublishEndpoint _publish;

        public InMemoryBrokerPublisher(IPublishEndpoint publish, ILogger<InMemoryBrokerPublisher> logger)
            : base(logger)
        {
            _publish = publish;
        }

        protected override Task PublishInternalAsync<T>(string topicName, T message)
        {
            return _publish.Publish(message);
        }
    }
}