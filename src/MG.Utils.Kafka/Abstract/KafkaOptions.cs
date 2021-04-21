using Confluent.Kafka;
using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.Extensions.Configuration;

namespace MG.Utils.Kafka.Abstract
{
    public class KafkaOptions
    {
        protected IConfigurationSection Settings { get; }

        protected IConfigurationSection Topics { get; }

        public KafkaOptions(IConfiguration configuration)
        {
            Settings = configuration.GetSection("MessageBroker");
            Topics = Settings.GetSection("Topics");
        }

        public NonNullableString TopicByName(string name)
        {
            return new NonNullableString(
                Topics[name],
                $"_topics[\"{name}\"]");
        }

        public ProducerConfig ProducerConfig()
        {
            var host = new NonNullableString(
                Settings["Host"],
                "_settings[\"Host\"]");

            var messageTimeout = new NonNullableInt(Settings["MessageTimeoutMs"]);

            return new ProducerConfig
            {
                BootstrapServers = host.Value(),
                MessageTimeoutMs = messageTimeout.ToInt()
            };
        }

        public ConsumerConfig ConsumerConfig()
        {
            return new ConsumerConfig(ProducerConfig())
            {
                GroupId = new NonNullableString(Settings["GroupId"]).Value(),
                AllowAutoCreateTopics = true
            };
        }
    }
}