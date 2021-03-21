using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Utils.Helpers;
using WebHost.Infrastructure.Contracts.MessageBroker;
using WebHost.Infrastructure.Infrastructure.MessageBrokers;

namespace WebHost.Infrastructure.Azure.ServiceBus
{
    public class AzureServiceBusPublisher : BrokerPublisherBase
    {
        private readonly MessageBrokerSettings _config;

        public AzureServiceBusPublisher(MessageBrokerSettings configuration, ILogger<AzureServiceBusPublisher> logger)
            : base(logger)
        {
            _config = configuration;
        }

        protected override async Task PublishInternalAsync<T>(string topicName, T message)
        {
            // create a Service Bus client
            await using var client = new ServiceBusClient(_config.Connection.ToString());

            ServiceBusSender sender = client.CreateSender(topicName);

            // create a message that we can send
            // send the message
            await sender.SendMessageAsync(
                new ServiceBusMessage(message.AsJson()));
        }
    }
}