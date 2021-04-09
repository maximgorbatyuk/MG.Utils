using System.Threading.Tasks;
using MG.Utils.Abstract;
using MG.Utils.Helpers;
using MG.Utils.Validation;
using MimeKit;

namespace MG.Utils.Smtp
{
    public class MailboxSmtpProvider : IEmail
    {
        private readonly SmtpEmailSettings _emailSettings;

        public MailboxSmtpProvider(SmtpEmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public Task SendAsync(string serializedMessage)
        {
            return SendAsync(serializedMessage.DeserializeAs<EmailMessage>());
        }

        public async Task SendAsync(EmailMessage message)
        {
            message.ThrowIfNull(nameof(message));
            message.ThrowIfInvalid();

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSettings.UserName, _emailSettings.UserName));

            foreach (var email in message.To)
            {
                // TODO Maxim: investigate what is 'name' param
                mimeMessage.To.Add(new MailboxAddress((string)null, email));
            }

            mimeMessage.Subject = message.Subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = message.Body
            };

            using var client = new MailKit.Net.Smtp.SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };

            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
            await client.ConnectAsync(
                host: _emailSettings.Server,
                port: _emailSettings.Port,
                useSsl: false);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            // Note: only needed if the SMTP server requires authentication
            await client.AuthenticateAsync(
                userName: _emailSettings.UserName,
                password: _emailSettings.Password);

            await client.SendAsync(mimeMessage);

            await client.DisconnectAsync(true);
        }
    }
}