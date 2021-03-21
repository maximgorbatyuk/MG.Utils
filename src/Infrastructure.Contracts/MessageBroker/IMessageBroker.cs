using System.Threading.Tasks;

namespace WebHost.Infrastructure.Contracts.MessageBroker
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(string topicName, T message)
            where T : class;
    }
}