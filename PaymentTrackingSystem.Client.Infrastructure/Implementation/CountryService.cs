using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class CountryService : ICountryService   
    {
        public HttpClient httpClient { get; set; }
        public string countryApiPath = "api/Country/";
        public CountryService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<CountryViewModel>> GetCountries()
        {
            var response = await httpClient.GetAsync(countryApiPath + "GetAllCountries");
            return await ApiStatusCodeHandler.HandleResponse<List<CountryViewModel>>(response);
        }


    }
}
