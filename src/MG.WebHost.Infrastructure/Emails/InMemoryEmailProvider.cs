using System.Threading.Tasks;
using MG.Utils.Helpers;
using MG.WebHost.Infrastructure.Contracts.ServiceBusMessages;
using Microsoft.Extensions.Logging;

namespace MG.WebHost.Infrastructure.Emails
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