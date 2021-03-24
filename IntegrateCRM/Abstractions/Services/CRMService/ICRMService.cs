using IntegrateCRM.Abstractions.Services.CRMService.Models;
using IntegrateCRM.Services;
using RestSharp;
using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.Services.CRMService
{
    public interface ICRMService
    {
        Task<IRestResponse<ClientResultResponse>> CreateClient(CreateCRMClientModel model);
        Task<IRestResponse<ClientResultResponse>> Register(RegisterCRMModel model);
    }
}
