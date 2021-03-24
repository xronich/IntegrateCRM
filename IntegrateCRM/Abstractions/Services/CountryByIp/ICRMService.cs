using IntegrateCRM.Abstractions.Services.CountryByIp.Models;
using System.Threading.Tasks;

namespace IntegrateCRM.Abstractions.Services.CountryByIp
{
    public interface ICountryByIpService
    {
        Task<CountryInfo> GetCountry(string requestUserHostAddress);
    }
}
