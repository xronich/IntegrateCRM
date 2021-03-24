using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using IntegrateCRM.Configuration;
using IntegrateCRM.Abstractions.Services.SmtpClientService;
using IntegrateCRM.Abstractions.Models;
using System.Text.Json;
using System.Net.Http;
using System;
using IntegrateCRM.Abstractions.DB;
using IntegrateCRM.Database.Entity;
using IntegrateCRM.Abstractions.Services.CRMService;

namespace IntegrateCRM.Controllers.EmailController
{
    [ApiController]
    [Route("api/public/contact-us")]
    public class ContactUsController : EmailControllerBase
    {
        private readonly IGoogleReCaptchaService _googleReCaptchaService;
        private readonly IMailChimpService _mailChimpService;
        private readonly ICRMDBContext _ICRMDBContext;
        public ContactUsController(
            IOptions<EmailProvider> emailProviderAccessor, 
            IOptions<EmailTemplate> emailTemplateAccessor,
            ISmtpClientService smtpClientService,
            IMailChimpService mailChimpService,
            IGoogleReCaptchaService googleReCaptchaService
            ICRMDBContext CRMDBContext
            ) : base (emailProviderAccessor, emailTemplateAccessor, smtpClientService)
        {
            _mailChimpService = mailChimpService;
            _googleReCaptchaService = googleReCaptchaService;
            _ICRMDBContext = CRMDBContext;
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUsModel model)
        {
            model.Ip = GetRemoteIpAddress();

            Request.Headers.TryGetValue("GoogleCaptchaResponseKey", out var googleCaptchaResponseKey);

            try
            {
                await _googleReCaptchaService.Validate(googleCaptchaResponseKey.ToString());
            }
            catch (Exception ex)
            {
                var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                httpResponseMessage.Content = new StringContent(ex.Message);

                return StatusCode(400, JsonSerializer.Serialize(ex.Message));
            }

            await SendMessage(model);

            var mailChimpResponse = await _mailChimpService.Subscribe(model);

            await DBInsert(model);

            return StatusCode((int)mailChimpResponse.StatusCode, mailChimpResponse.StatusCode != System.Net.HttpStatusCode.OK  ? mailChimpResponse.Content : null);
        }

        [HttpPost]
        [Route("no-captcha")]
        public async Task<IActionResult> ContactUsNoCaptcha(ContactUsModel model)
        {
            model.Ip = GetRemoteIpAddress();

            await SendMessage(model);

            var mailChimpResponse = await _mailChimpService.Subscribe(model);

            await DBInsert(model);

            return StatusCode((int)mailChimpResponse.StatusCode, mailChimpResponse.StatusCode != System.Net.HttpStatusCode.OK ? mailChimpResponse.Content : null);
        }

        [HttpGet]
        [Route("config")]
        public async Task<ContactUsSettingResponseModel> GetConfig()
        {
            var config = new ContactUsSettingResponseModel
            {
                SenderName = _emailProvider.Name,
                ReceiverEmails = _emailProvider.ReceiverEmails,
                SenderEmail = _emailProvider.SenderEmail
            };

            return config;
        }

        private async Task DBInsert(ContactUsModel model)
        {
            await _ICRMDBContext.Current.Insert(new EmailMessage
            {
                Country = model.Country,
                Email = model.Email,
                Ip = model.Ip,
                Company = model.Company,
                Message = model.Message,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            });
        }
    }
}
