using MG.Utils.Azure.ServiceBus.Constracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MG.Utils.Azure.ServiceBus.Extensions
{
    public static class AzureServiceBusExtensions
    {
        public static IHealthChecksBuilder AddAzureServiceBusHealthCheck(
            this IHealthChecksBuilder services, IConfiguration config)
        {
            var configuration = new MessageBrokerSettings(config);
            services
                .AddAzureServiceBusTopic(
                    connectionString: configuration.HealthCheckConnection,
                    topicName: configuration.HealthCheckTopic);

            return services;
        }
    }
}