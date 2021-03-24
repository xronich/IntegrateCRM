using IntegrateCRM.Abstractions.Services.SmtpClientService;
using IntegrateCRM.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IntegrateCRM.Services
{
    public class SmtpClientService : ISmtpClientService
    {
        private readonly EmailProvider _emailProvider;
        private SmtpClient client;

        public SmtpClientService(IOptions<EmailProvider> emailProviderAccessor)
        {
            _emailProvider = emailProviderAccessor.Value;

            Init();
        }

        private void Init()
        {
            client = new SmtpClient();

            client.Port = _emailProvider.Port;
            client.Host = _emailProvider.Host;
            client.Credentials = new NetworkCredential(_emailProvider.Login, _emailProvider.Password);
            client.EnableSsl = _emailProvider.UseSsl;
        }

        public async Task Send(MailMessage message)
        {
            await client.SendMailAsync(message);
        }
    }
}
