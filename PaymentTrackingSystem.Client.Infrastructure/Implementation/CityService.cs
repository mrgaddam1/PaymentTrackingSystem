using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class CityService : ICityService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/City/";

        public CityService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<CityViewModel>> GetAllCities()
        {
            var response = await httpClient.GetAsync(ApiPath + "GetAllCities");
            if (!response.IsSuccessStatusCode)
            {
                return new List<CityViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<CityViewModel>>() ?? new List<CityViewModel>();
        }

        public async Task<CityViewModel?> GetCityDetailsById(int cityId)
        {
            var response = await httpClient.GetAsync($"{ApiPath}GetCityDetailsById?cityId={cityId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CityViewModel>();
        }

        public async Task<bool> Add(CityViewModel cityViewModel)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath + "Add", cityViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(CityViewModel cityViewModel)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPath + "Update", cityViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int cityId)
        {
            var response = await httpClient.DeleteAsync($"{ApiPath}Delete?cityId={cityId}");
            return response.IsSuccessStatusCode;
        }
    }
}
