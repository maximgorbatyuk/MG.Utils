using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils.ValueObjects;
using WebHost.Infrastructure.Azure.ServiceBus;
using WebHost.Infrastructure.Contracts.MessageBroker;
using WebHost.Infrastructure.Infrastructure.MassTransitConsumers;

namespace WebHost.Infrastructure.Config
{
    public class MessageBrokerConfig
    {
        private readonly IServiceCollection _services;
        private readonly Bool _useInMemoryMb;
        private readonly MessageBrokerSettings _configuration;

        public MessageBrokerConfig(
            IServiceCollection services,
            IConfiguration configuration)
        {
            _services = services;
            _useInMemoryMb = new Bool(configuration["UseInMemoryMessageBroker"]);
            _configuration = new MessageBrokerSettings(configuration);
        }

        public MessageBrokerConfig Setup()
        {
            _services.AddTransient<MessageBrokerSettings>();
            return _useInMemoryMb.ToBool() ? InMemoryMb() : AzureServiceBus();
        }

        private MessageBrokerConfig InMemoryMb()
        {
            _services.AddMassTransit(x =>
            {
                x.AddConsumer<MassTransitEmailSendConsumer>();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.TransportConcurrencyLimit = 100;
                    cfg.ConfigureEndpoints(context);

                    cfg.ReceiveEndpoint(_configuration.EmailMessageTopic, e =>
                    {
                        e.ConfigureConsumer<MassTransitEmailSendConsumer>(context);
                    });
                });
            });

            _services.AddMassTransitHostedService();
            _services.AddScoped<IMessageBroker, InMemoryBrokerPublisher>();
            return this;
        }

        private MessageBrokerConfig AzureServiceBus()
        {
            _services.AddHostedService<AzureBrokerEmailConsumerBackService>();
            _services.AddScoped<IMessageBroker, AzureServiceBusPublisher>();

            _services
                .AddHealthChecks()
                .AddAzureServiceBusTopic(
                    connectionString: _configuration.HealthCheckConnection.ToString(),
                    topicName: _configuration.HealthCheckTopic.ToString());

            return this;
        }
    }
}