using Microsoft.Extensions.Configuration;
using Utils.ValueObjects;

namespace WebHost.Infrastructure.Contracts.MessageBroker
{
    public class MessageBrokerSettings
    {
        public NonNullableString Connection { get; }

        public NonNullableString EmailMessageTopic { get; }

        public NonNullableString HealthCheckConnection { get; }

        public NonNullableString HealthCheckTopic { get; }

        public MessageBrokerSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("Azure").GetSection("ServiceBus");
            Connection = new NonNullableString(section[nameof(Connection)]);
            EmailMessageTopic = new NonNullableString(section[nameof(EmailMessageTopic)]);

            HealthCheckConnection = new NonNullableString(section[nameof(HealthCheckConnection)]);
            HealthCheckTopic = new NonNullableString(section[nameof(HealthCheckTopic)]);
        }
    }
}