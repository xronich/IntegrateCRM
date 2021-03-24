using System.Net.Mail;
using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.Services.SmtpClientService
{
    public interface ISmtpClientService
    {
        Task Send(MailMessage message);
    }
}
