using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.Services.SmtpClientService
{
    public interface IGoogleReCaptchaService
    {
        Task Validate(string googleResponse);
    }
}
