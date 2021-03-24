using IntegrateCRM.Abstractions.Services.CountryByIp;
using IntegrateCRM.Abstractions.Services.CountryByIp.Models;
using IntegrateCRM.Configuration;
using IntegrateCRM.Services.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrateCRM.Services
{
    public class CountryByIpService : ICountryByIpService
    {
        private readonly HttpClient _httpClient;
        private readonly CountryByIpConfiguration _countryByIpConfiguration;

        public CountryByIpService(IOptions<CountryByIpConfiguration> countryByIpConfigurationAccessor)
        {
            _countryByIpConfiguration = countryByIpConfigurationAccessor.Value;
            _httpClient = new HttpClient();
        }

        public async Task<CountryInfo> GetCountry(string requestUserHostAddress)
        {
            var formattableString =
                $"{_countryByIpConfiguration.ServiceUri.Scheme}://{_countryByIpConfiguration.ServiceUri.Host}{_countryByIpConfiguration.ServiceUri.AbsolutePath}?ip={requestUserHostAddress}";
            var httpResponseMessage = await _httpClient.GetAsync(formattableString);

            var jObject = await httpResponseMessage.Content.ReadAsAsync<JObject>();
            var geoServiceResponseModel = jObject.ToObject<GeoServiceResponseModel>();

            return new CountryInfo()
            {
                CountryCode = geoServiceResponseModel.CountryCode,
                CountryName = geoServiceResponseModel.CountryName,
                CountryPhoneCode = geoServiceResponseModel.CountryPhoneCode
            };
        }
    }
}
