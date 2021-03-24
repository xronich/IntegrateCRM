using IntegrateCRM.Abstractions.Models;
using IntegrateCRM.Services;
using RestSharp;
using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.Services.CRMService
{
    public interface IMailChimpService
    {
        Task<IRestResponse<ClientResultResponse>> Subscribe(ContactUsModel model);
    }
}
