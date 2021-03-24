using System.Threading.Tasks;
using MoreLinq;
using Microsoft.Extensions.Options;
using System.Text;
using System.Net.Mail;
using System.Reflection;
using System.Linq;
using IntegrateCRM.Abstractions.Services.SmtpClientService;
using IntegrateCRM.Configuration;
using IntegrateCRM.Abstractions.Models;

namespace IntegrateCRM.Controllers.EmailController
{

    public class EmailControllerBase : AppControllerBase
    {
        protected readonly EmailProvider _emailProvider;
        private readonly EmailTemplate _emailTemplate;
        private readonly ISmtpClientService _smtpClientService;
        public EmailControllerBase( 
            IOptions<EmailProvider> emailProviderAccessor, 
            IOptions<EmailTemplate> emailTemplateAccessor,
            ISmtpClientService smtpClientService)
        {
            _emailTemplate = emailTemplateAccessor.Value;
            _emailProvider = emailProviderAccessor.Value;
            _smtpClientService = smtpClientService;
        }

        protected async Task SendMessage(ContactUsModel model)
        {
            var stringBuilder = new StringBuilder(_emailTemplate.Body);

            var fields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            fields.ForEach(f => stringBuilder.Replace($"{_emailTemplate.VariablePrefix}{f?.Name}", (f.GetValue(model) ?? string.Empty).ToString()));

            var mailMessage = new MailMessage(new MailAddress(_emailProvider.SenderEmail, _emailProvider.Name), new MailAddress(_emailProvider.ReceiverEmails.Split(',').First()));
            mailMessage.Bcc.Add(_emailProvider.ReceiverEmails);
            mailMessage.Subject = _emailTemplate.Subject;
            mailMessage.Body = stringBuilder.ToString();

            await _smtpClientService.Send(mailMessage);
        }
    }
}
