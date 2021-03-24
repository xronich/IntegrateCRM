using IntegrateCRM.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using RestSharp;
using IntegrateCRM.Abstractions.Services.CRMService.Models;
using IntegrateCRM.Abstractions.Services.CRMService;
using IntegrateCRM.Extensions;

namespace IntegrateCRM.Services
{
    public class CRMService : ICRMService
    {
        private readonly CRMConfiguration _CRMConfiguration;
        private RestClient client;

        public CRMService(IOptions<CRMConfiguration> CRMConfigurationAccessor)
        {
            _CRMConfiguration = CRMConfigurationAccessor.Value;

            client = new RestClient(_CRMConfiguration.Url);
        }

        public async Task<IRestResponse<ClientResultResponse>> CreateClient(CreateCRMClientModel model)
        {
            var request = new RestRequest($"/api/users/trading-platform/create-client?api_key={_CRMConfiguration.ApiKey}", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("country", model.Country);
            request.AddParameter("currency_code", model.Currency_code);
            request.AddParameter("email", model.Email);
            request.AddParameter("comment", model.Comment);
            request.AddParameter("first_name", model.FirstName);
            request.AddParameter("last_name", model.LastName);
            request.AddParameter("phone", model.Phone);
            request.AddParameter("gmt_timezone", model.GmtTimezone.IsNullOrEmpty() ? "12:00" : model.GmtTimezone);
            request.AddParameter("password", model.Password);
            request.AddParameter("lang", model.Lang);
            request.AddParameter("campaign_id", model.CampaignId.IsNullOrEmpty() ? _CRMConfiguration.CampaignId : model.CampaignId);
            request.AddParameter("free_text", model.FreeText);

            var result = client.Execute<ClientResultResponse>(request);

            return result;
        }

        public async Task<IRestResponse<ClientResultResponse>> Register(RegisterCRMModel model)
        {
            var request = new RestRequest($"/api/users/trading-platform/register", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("country", model.Country);
            request.AddParameter("currency_code", model.Currency_code);
            request.AddParameter("email", model.Email);
            //request.AddParameter("promo_code", model.PromoCode);
            request.AddParameter("first_name", model.FirstName);
            request.AddParameter("last_name", model.LastName);
            request.AddParameter("phone", model.Phone);
            request.AddParameter("gmt_timezone", model.GmtTimezone.IsNullOrEmpty() ? "12:00" : model.GmtTimezone);
            request.AddParameter("password", model.Password);
            request.AddParameter("note", model.Message);
            request.AddParameter("lang", model.Lang);
            request.AddParameter("campaign_id", model.CampaignId.IsNullOrEmpty() ? _CRMConfiguration.CampaignId : model.CampaignId);
            request.AddParameter("free_text", model.FreeText);

            var result = client.Execute<ClientResultResponse>(request);

            return result;
        }
    }
    public class ResultResponse
    {
        public string Result { get; set; }
        public string Msg { get; set; }
    }

    public class ClientResultResponse : ResultResponse
    {
        public string client_id { get; set; }
    }
}
