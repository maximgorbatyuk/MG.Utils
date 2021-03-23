using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MG.Utils.Abstract;
using MG.Utils.Azure.ServiceBus.Constracts;
using MG.Utils.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MG.Utils.Azure.ServiceBus
{
    public class AzureServiceBusPublisher : IMessageBroker
    {
        private readonly MessageBrokerSettings _config;
        private readonly ILogger _logger;

        public AzureServiceBusPublisher(IConfiguration configuration, ILogger<AzureServiceBusPublisher> logger)
        {
            _config = new MessageBrokerSettings(configuration);
            _logger = logger;
        }

        public async Task PublishAsync<T>(string topicName, T message)
            where T : class
        {
            topicName.ThrowIfNullOrEmpty(nameof(topicName));
            message.ThrowIfNull(nameof(message));

            try
            {
                _logger.LogDebug($"Publishing message {message}");

                // create a Service Bus client
                await using var client = new ServiceBusClient(_config.Connection.ToString());

                ServiceBusSender sender = client.CreateSender(topicName);

                // create a message that we can send
                // send the message
                await sender.SendMessageAsync(
                    new ServiceBusMessage(message.AsJson()));

                _logger.LogDebug($"Sent a single message to the queue: {message}");
            }
            catch
            {
                _logger.LogError($"The message {message} was not published due to error");
                throw;
            }
        }
    }
}