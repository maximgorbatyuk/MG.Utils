using System.Threading.Tasks;

namespace MG.Utils.Azure.ServiceBus.Constracts
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(string topicName, T message)
            where T : class;
    }
}