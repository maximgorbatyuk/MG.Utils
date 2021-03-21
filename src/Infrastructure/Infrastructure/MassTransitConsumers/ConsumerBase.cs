using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using WebHost.Infrastructure.Contracts;

namespace WebHost.Infrastructure.Infrastructure.MassTransitConsumers
{
    public abstract class ConsumerBase<T> : IConsumer<T>
        where T : class
    {
        protected ConsumerBase(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected abstract Task ConsumeAsync(ConsumeContext<T> context);

#pragma warning disable UseAsyncSuffix // Use Async suffix
        public async Task Consume(ConsumeContext<T> context)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            var messageType = typeof(T).Name;
            try
            {
                Logger.LogInformation($"Consuming {messageType}");
                await ConsumeAsync(context);
                Logger.LogInformation($"Consumed {messageType}");
            }
            catch (Exception e)
            {
                Logger.LogError(
                    eventId: EventIdFactory.ConsumerError,
                    exception: e,
                    message: $"An error during consuming {messageType}:{Environment.NewLine}{e.Message}");
            }
        }
    }
}