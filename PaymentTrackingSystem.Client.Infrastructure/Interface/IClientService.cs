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
        Task<ClientViewModel> GetClientDetailsById();
        Task<bool> Add();
        Task<bool> Update();
        Task<bool> Delete();
    }
}
