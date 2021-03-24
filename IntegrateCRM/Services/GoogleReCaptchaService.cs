using ContactUs.Models;
using IntegrateCRM.Abstractions.Services.SmtpClientService;
using IntegrateCRM.Configuration;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IntegrateCRM.Services
{
    public class GoogleReCaptchaService : IGoogleReCaptchaService
    {
        private readonly GoogleCaptcha _googleCaptcha;

        public GoogleReCaptchaService(IOptions<GoogleCaptcha> googleCaptchaAccessor)
        {
            _googleCaptcha = googleCaptchaAccessor.Value;
        }

        public async Task Validate(string googleResponse)
        {
            var client = new RestClient(_googleCaptcha.ApiUrl);

            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("secret", _googleCaptcha.SecretKey);
            request.AddParameter("response", googleResponse);

            var result = await client.ExecuteAsync<GoogleCaptchaResultResponse>(request);

            if (!result.Data.Success)
            {
                throw new Exception($"Google cpatcha error: {String.Join(", ", result.Data.ErrorCodes)}");
            }

            if (result.Data.Score < 0.7)
            {
                throw new Exception($"Google cpatcha score too low");
            }
        }
    }
}
