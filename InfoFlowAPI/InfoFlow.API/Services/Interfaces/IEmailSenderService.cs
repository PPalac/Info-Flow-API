using System.Threading.Tasks;

namespace InfoFlow.API.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmail(string email, string subject, string message);
        Task Execute(string apiKey, string subject, string message, string email);

    }
}
