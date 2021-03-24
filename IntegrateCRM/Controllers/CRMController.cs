using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IntegrateCRM.Abstractions.Services.CRMService;
using IntegrateCRM.Abstractions.Services.CRMService.Models;
using IntegrateCRM.Abstractions.DB;
using IntegrateCRM.Extensions;
using System.Net;
using IntegrateCRM.Abstractions.Services.CountryByIp;
using IntegrateCRM.Database.Entity;
using System.Net.Http;
using System;
using IntegrateCRM.Controllers.EmailController;
using IntegrateCRM.Abstractions.Services.SmtpClientService;
using System.Text.Json;
using Microsoft.Extensions.Options;
using IntegrateCRM.Configuration;
using IntegrateCRM.Abstractions.Models;

namespace IntegrateCRM.Controllers
{
    [ApiController]
    [Route("api/public/crm")]
    public class CRMController : EmailControllerBase
    {
        private readonly ICRMService _CRMService;
        private readonly ICRMDBContext _ICRMDBContext;
        private readonly ICountryByIpService _CountryByIpService;
        private readonly IGoogleReCaptchaService _googleReCaptchaService;
        public CRMController(IOptions<EmailProvider> emailProviderAccessor,
            IOptions<EmailTemplate> emailTemplateAccessor,
            ISmtpClientService smtpClientService, 
            ICRMService CRMService, 
            ICountryByIpService CountryByIpService, 
            ICRMDBContext DBContext, 
            IGoogleReCaptchaService googleReCaptchaService) 
            : base(emailProviderAccessor, emailTemplateAccessor, smtpClientService)
        {
            _googleReCaptchaService = googleReCaptchaService;
            _CRMService = CRMService;
            _ICRMDBContext = DBContext;
            _CountryByIpService = CountryByIpService;
        }

        [HttpPost]
        [Route("create-client")]
        public async Task<IActionResult> CreateClient(CreateCRMClientModel model)
        {
            model.Ip = GetRemoteIpAddress();

            model.Country = model.Country.IsNullOrEmptyOrNone() 
                ? await GetCountryName(model.Ip)
                : model.Country;

            var response = await _CRMService.CreateClient(model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await _ICRMDBContext.Current.Insert(new Client
                {
                    Country = model.Country,
                    CurrencyCode = model.Currency_code,
                    CampaignId = model.CampaignId,
                    Comment = model.Comment,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    FreeText = model.FreeText,
                    GmtTimezone = model.GmtTimezone,
                    Ip = model.Ip,
                    ClientId = response.Data.client_id,
                    Lang = model.Lang,
                    LastName = model.LastName,
                    Password = model.Password,
                    Phone = model.Phone
                });
            }

            return StatusCode((int)response.StatusCode, response.Content);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterCRMModel model)
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

            model.Country = model.Country.IsNullOrEmptyOrNone()
                ? await GetCountryName(model.Ip)
                : model.Country;

            var response = await _CRMService.Register(model);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await _ICRMDBContext.Current.Insert(new Registration
                {
                    Country = model.Country,
                    CurrencyCode = model.Currency_code,
                    CampaignId = model.CampaignId,
                    PromoCode = model.PromoCode,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    FreeText = model.FreeText,
                    GmtTimezone = model.GmtTimezone,
                    Ip = model.Ip,
                    ClientId = response.Data.client_id,
                    Lang = model.Lang,
                    LastName = model.LastName,
                    Password = model.Password,
                    Phone = model.Phone
                });
            }
            else
            {
                return StatusCode((int)response.StatusCode, response.Content);
            }

            var contactUsModel = new ContactUsModel
            {
                Company = model.Company,
                Country = model.Country,
                Email = model.Email,
                Ip = model.Ip,
                Message = model.Message,
                Name = $"{model.FirstName} {model.LastName}",
                PhoneNumber = model.PromoCode,
                FreeText = model.FreeText
            };

            await SendMessage(contactUsModel);

            await DBInsert(contactUsModel);

            return StatusCode(204);
        }

        private async Task<string> GetCountryName(string ip)
        {
            var countryInfo = await _CountryByIpService.GetCountry(ip);

            return countryInfo.CountryName;
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
