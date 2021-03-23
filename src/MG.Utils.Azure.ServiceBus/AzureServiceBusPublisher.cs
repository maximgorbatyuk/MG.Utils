using System.Threading.Tasks;
using MG.Utils.Azure.ServiceBus.Constracts;

namespace MG.Utils.Azure.ServiceBus
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