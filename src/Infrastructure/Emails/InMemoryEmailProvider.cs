using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utils.Helpers;
using WebHost.Infrastructure.Contracts.ServiceBusMessages;

namespace WebHost.Infrastructure.Emails
{
    public class InMemoryEmailProvider : IEmail
    {
        private readonly ILogger<InMemoryEmailProvider> _logger;

        public InMemoryEmailProvider(ILogger<InMemoryEmailProvider> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string serializedMessage)
        {
            return SendAsync(serializedMessage.DeserializeAs<EmailMessage>());
        }

        public Task SendAsync(EmailMessage message)
        {
            _logger.LogInformation(message.DebugInfo());
            return Task.CompletedTask;
        }
    }
}