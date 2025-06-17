using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class ClientService : IClientService
    {
        public HttpClient httpClient { get; set; }

        public ClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<bool> Add(ClientViewModel clientViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/Client/Add", clientViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return isSuccess;
            }
        }


        public Task<bool> Delete(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetAllClients<T>()
        {
            var response = await httpClient.GetAsync("api/Client/GetAllClients");
            return await ApiStatusCodeHandler.HandleResponse<T>(response);
        }

        public Task<ClientViewModel> GetClientDetailsById(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ClientViewModel clientViewModel)
        {
            bool isSuccess = false;
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/Client/Update", clientViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return isSuccess;
            }
        }
    }
}
