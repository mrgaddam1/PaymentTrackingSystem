using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class TenantService : ITenantService
    {
        private readonly HttpClient httpClient;

        public TenantService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<TenantViewModel>> GetAllTenants()
        {
            try
            {
                var response = await httpClient.GetAsync("api/Tenant/GetAllTenants");
                return await ApiStatusCodeHandler.HandleResponse<List<TenantViewModel>>(response) ?? new List<TenantViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tenants: {ex.Message}");
                return new List<TenantViewModel>();
            }
        }

        public async Task<TenantViewModel?> GetTenantDetailsById(int tenantId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Tenant/GetTenantDetailsById?tenantId={tenantId}");
                return await ApiStatusCodeHandler.HandleResponse<TenantViewModel>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tenant: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Add(TenantViewModel tenantViewModel)
        {
            return await SendWriteRequest(() => httpClient.PostAsJsonAsync("api/Tenant/Add", tenantViewModel));
        }

        public async Task<bool> Update(TenantViewModel tenantViewModel)
        {
            return await SendWriteRequest(() => httpClient.PutAsJsonAsync("api/Tenant/Update", tenantViewModel));
        }

        public async Task<bool> Delete(int tenantId)
        {
            return await SendWriteRequest(() => httpClient.DeleteAsync($"api/Tenant/Delete?tenantId={tenantId}"));
        }

        private static async Task<bool> SendWriteRequest(Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                using var response = await request();
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tenant: {ex.Message}");
                return false;
            }
        }
    }
}
