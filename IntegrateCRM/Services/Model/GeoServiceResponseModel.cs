using Newtonsoft.Json;

namespace IntegrateCRM.Services.Model
{
    public class GeoServiceResponseModel
    {
        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("phoneCode")]
        public string CountryPhoneCode { get; set; }
    }
}
