using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class DistrictService : IDistrictService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/District/";

        public DistrictService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<DistrictViewModel>> GetAllDistricts()
        {
            var response = await httpClient.GetAsync(ApiPath + "GetAllDistricts");
            if (!response.IsSuccessStatusCode)
            {
                return new List<DistrictViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<DistrictViewModel>>() ?? new List<DistrictViewModel>();
        }

        public async Task<DistrictViewModel?> GetDistrictDetailsById(int districtId)
        {
            var response = await httpClient.GetAsync($"{ApiPath}GetDistrictDetailsById?districtId={districtId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DistrictViewModel>();
        }

        public async Task<bool> Add(DistrictViewModel districtViewModel)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath + "Add", districtViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(DistrictViewModel districtViewModel)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPath + "Update", districtViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int districtId)
        {
            var response = await httpClient.DeleteAsync($"{ApiPath}Delete?districtId={districtId}");
            return response.IsSuccessStatusCode;
        }
    }
}
