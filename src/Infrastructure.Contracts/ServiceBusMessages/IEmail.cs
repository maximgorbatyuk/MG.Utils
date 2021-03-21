using System.Threading.Tasks;

namespace WebHost.Infrastructure.Contracts.ServiceBusMessages
{
    public interface IEmail
    {
        Task SendAsync(string serializedEmailMessage);

        Task SendAsync(EmailMessage message);
    }
}