using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MG.Utils.ValueObjects;
using MG.WebHost.Infrastructure.Contracts.MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MG.WebHost.Infrastructure.Azure.ServiceBus
{
    public abstract class AzureBusTopicConsumerBase : BackgroundService
    {
        private const int DefaultDelay = 3000;

        private readonly ILogger _logger;

        protected IServiceScopeFactory ScopeFactory { get; }

        protected MessageBrokerSettings BrokerSettings { get; }

        protected abstract NonNullableString MessageTopic { get; }

        protected AzureBusTopicConsumerBase(
            ILogger logger, IServiceScopeFactory scopeFactory, MessageBrokerSettings brokerSettings)
        {
            _logger = logger;
            ScopeFactory = scopeFactory;
            BrokerSettings = brokerSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var client = new ServiceBusClient(BrokerSettings.Connection);
            await using var processor = client.CreateProcessor(
                queueName: MessageTopic,
                options: new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += MessageHandlerAsync;
            processor.ProcessErrorAsync += ErrorHandlerAsync;

            _logger.LogInformation("Starting consuming");
            await processor.StartProcessingAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(DefaultDelay, stoppingToken);
            }

            _logger.LogInformation("Stopping consuming...");
            await processor.StopProcessingAsync(stoppingToken);
        }

        protected abstract Task MessageHandleInternalAsync(
            IServiceProvider provider, ServiceBusReceivedMessage message);

        // handle received messages
        private async Task MessageHandlerAsync(ProcessMessageEventArgs args)
        {
            _logger.LogInformation("Received email message");

            using var scope = ScopeFactory.CreateScope();

            await MessageHandleInternalAsync(scope.ServiceProvider, args.Message);

            // complete the message. messages is deleted from the queue.
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        private Task ErrorHandlerAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}