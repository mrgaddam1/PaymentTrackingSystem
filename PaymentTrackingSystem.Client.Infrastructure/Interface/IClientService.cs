using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IClientService
    {
        Task<T?> GetAllClients<T>();
        Task<ClientViewModel> GetClientDetailsById(int clientId);
        Task<bool> Add(ClientViewModel clientViewModel);
        Task<bool> Update(ClientViewModel clientViewModel);
        Task<bool> Delete(int clientId);
    }
}
