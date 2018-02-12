using System.Threading.Tasks;
using InfoFlow.API.Helpers;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace InfoFlow.API.Services.Interfaces
{
    public class EmailSenderService : IEmailSenderService
    {
        private AuthMessageSenderOptions options;

        public EmailSenderService(IOptions<AuthMessageSenderOptions> options)
        {
            this.options = options.Value;
        }

        public Task SendEmail(string email, string subject, string message)
        {
            return Execute(options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("office@polibuda.pl", "Politechnika XYZ"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }

    }
}
