using System.Net.Http.Json;
using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class StateService : IStateService
    {
        private readonly HttpClient httpClient;
        private const string ApiPath = "api/State/";

        public StateService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<StateViewModel>> GetAllStates()
        {
            var response = await httpClient.GetAsync(ApiPath + "GetAllStates");
            if (!response.IsSuccessStatusCode)
            {
                return new List<StateViewModel>();
            }

            return await response.Content.ReadFromJsonAsync<List<StateViewModel>>() ?? new List<StateViewModel>();
        }

        public async Task<StateViewModel?> GetStateDetailsById(int stateId)
        {
            var response = await httpClient.GetAsync($"{ApiPath}GetStateDetailsById?stateId={stateId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<StateViewModel>();
        }

        public async Task<bool> Add(StateViewModel stateViewModel)
        {
            var response = await httpClient.PostAsJsonAsync(ApiPath + "Add", stateViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(StateViewModel stateViewModel)
        {
            var response = await httpClient.PutAsJsonAsync(ApiPath + "Update", stateViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int stateId)
        {
            var response = await httpClient.DeleteAsync($"{ApiPath}Delete?stateId={stateId}");
            return response.IsSuccessStatusCode;
        }
    }
}
