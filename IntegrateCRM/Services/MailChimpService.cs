using IntegrateCRM.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using RestSharp;
using IntegrateCRM.Abstractions.Services.CRMService;
using RestSharp.Authenticators;
using IntegrateCRM.Abstractions.Models;

namespace IntegrateCRM.Services
{
    public class MailChimpService : IMailChimpService
    {
        private readonly MailChimpConfiguration _mailChimpConfiguration;
        private RestClient client;

        public MailChimpService(IOptions<MailChimpConfiguration> mailChimpConfiguration)
        {
            _mailChimpConfiguration = mailChimpConfiguration.Value;

            client = new RestClient(_mailChimpConfiguration.Url);
        }

        public async Task<IRestResponse<ClientResultResponse>> Subscribe(ContactUsModel model)
        {
            var mailChimpCreateMember = new MailChimpCreateMember
            {
                email_address = model.Email,
                merge_fields = new MergeFields
                {
                    FNAME = model.Name,
                    PHONE = model.PhoneNumber,
                    ADDRESS = model.Country
                }
            };

            client.Authenticator = new HttpBasicAuthenticator("MailChimp", _mailChimpConfiguration.ApiKey);

            var request = new RestRequest($"/lists/{_mailChimpConfiguration.ListId}/members", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(mailChimpCreateMember);

            var result = client.Execute<ClientResultResponse>(request);

            return result;
        }
    }
    public class MailChimpCreateMember
    {
        public string email_address { get; set; }
        public string status => "subscribed";
        public MergeFields merge_fields { get; set; }
    }
    public class MergeFields
    {
        public string FNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
    }
}
