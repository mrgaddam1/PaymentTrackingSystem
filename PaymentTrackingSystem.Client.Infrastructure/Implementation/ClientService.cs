using PaymentTrackingSystem.Client.Infrastructure.Interface;
using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentTrackingSystem.Common.ApplicationStatusCodeHandler;
namespace PaymentTrackingSystem.Client.Infrastructure.Implementation
{
    public class ClientService : IClientService
    {
        public HttpClient httpClient { get; set; }

        public ClientService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public Task<bool> Add()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetAllClients<T>()
        {
            var response = await httpClient.GetAsync("api/Client/GetAllClients");
            return await ApiStatusCodeHandler.HandleResponse<T>(response);
        }

        public Task<ClientViewModel> GetClientDetailsById()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update()
        {
            throw new NotImplementedException();
        }
    }
}
