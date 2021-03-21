using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utils.Helpers;
using WebHost.Infrastructure.Contracts.MessageBroker;

namespace WebHost.Infrastructure.Infrastructure.MessageBrokers
{
    public abstract class BrokerPublisherBase : IMessageBroker
    {
        private readonly ILogger _logger;

        protected BrokerPublisherBase(ILogger logger)
        {
            _logger = logger;
        }

        protected abstract Task PublishInternalAsync<T>(string topicName, T message)
            where T : class;

        public async Task PublishAsync<T>(string topicName, T message)
            where T : class
        {
            topicName.ThrowIfNullOrEmpty(nameof(topicName));
            message.ThrowIfNull(nameof(message));

            try
            {
                _logger.LogDebug($"Publishing message {message}");

                await PublishInternalAsync(topicName, message);

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