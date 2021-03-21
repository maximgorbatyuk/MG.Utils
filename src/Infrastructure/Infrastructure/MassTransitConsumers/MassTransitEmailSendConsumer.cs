using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using WebHost.Infrastructure.Contracts.ServiceBusMessages;

namespace WebHost.Infrastructure.Infrastructure.MassTransitConsumers
{
    public class MassTransitEmailSendConsumer : ConsumerBase<EmailMessage>
    {
        private readonly IEmail _email;

        protected override async Task ConsumeAsync(ConsumeContext<EmailMessage> context)
        {
            await _email.SendAsync(context.Message);
            Logger.LogDebug("Email sent");
        }

        public MassTransitEmailSendConsumer(ILogger<MassTransitEmailSendConsumer> logger, IEmail email)
            : base(logger)
        {
            _email = email;
        }
    }
}