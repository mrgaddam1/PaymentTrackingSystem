using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class TenantTypeService : ITenantTypeService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/TenantType/";

        public TenantTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TenantTypeViewModel>> GetAllTenantTypes()
        {
            using var response = await httpClient.GetAsync(ApiPath + "GetAllTenantTypes");
            if (!response.IsSuccessStatusCode)
            {
                return new List<TenantTypeViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<TenantTypeViewModel>>() ?? new List<TenantTypeViewModel>();
        }

        public async Task<TenantTypeViewModel?> GetTenantTypeDetailsById(int tenantTypeId)
        {
            using var response = await httpClient.GetAsync($"{ApiPath}GetTenantTypeDetailsById?tenantTypeId={tenantTypeId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TenantTypeViewModel>();
        }

        public async Task<bool> Add(TenantTypeViewModel tenantTypeViewModel)
        {
            using var response = await httpClient.PostAsJsonAsync(ApiPath + "Add", tenantTypeViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(TenantTypeViewModel tenantTypeViewModel)
        {
            using var response = await httpClient.PutAsJsonAsync(ApiPath + "Update", tenantTypeViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int tenantTypeId)
        {
            using var response = await httpClient.DeleteAsync($"{ApiPath}Delete?tenantTypeId={tenantTypeId}");
            return response.IsSuccessStatusCode;
        }
    }
}
