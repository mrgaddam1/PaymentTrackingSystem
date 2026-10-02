using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class AssignPropertyToTenantService : IAssignPropertyToTenantService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/AssignPropertyToTenant/";

        public AssignPropertyToTenantService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<AssignPropertyToTenantViewModel>> GetAllAssignments()
        {
            try
            {
                using var response = await httpClient.GetAsync(ApiPath + "GetAllAssignments");
                return await ApiStatusCodeHandler.HandleResponse<List<AssignPropertyToTenantViewModel>>(response)
                    ?? new List<AssignPropertyToTenantViewModel>();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error loading property assignments: {exception.Message}");
                return new List<AssignPropertyToTenantViewModel>();
            }
        }

        public async Task<AssignPropertyToTenantViewModel?> GetAssignmentById(int rentId)
        {
            try
            {
                using var response = await httpClient.GetAsync($"{ApiPath}GetAssignmentById?rentId={rentId}");
                return await ApiStatusCodeHandler.HandleResponse<AssignPropertyToTenantViewModel>(response);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error loading property assignment: {exception.Message}");
                return null;
            }
        }

        public async Task<bool> Add(AssignPropertyToTenantViewModel assignment)
        {
            return await SendWriteRequest(() => httpClient.PostAsJsonAsync(ApiPath + "Add", assignment));
        }

        public async Task<bool> Update(AssignPropertyToTenantViewModel assignment)
        {
            return await SendWriteRequest(() => httpClient.PutAsJsonAsync(ApiPath + "Update", assignment));
        }

        public async Task<bool> Delete(int rentId)
        {
            return await SendWriteRequest(() => httpClient.DeleteAsync($"{ApiPath}Delete?rentId={rentId}"));
        }

        private static async Task<bool> SendWriteRequest(Func<Task<HttpResponseMessage>> request)
        {
            try
            {
                using var response = await request();
                return response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error saving property assignment: {exception.Message}");
                return false;
            }
        }
    }
}
