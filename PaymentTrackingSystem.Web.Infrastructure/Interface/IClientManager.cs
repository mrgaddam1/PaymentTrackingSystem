using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IClientManager
    {
        Task<List<ClientViewModel>> GetAllClients();
        Task<ClientViewModel> GetClientDetailsById();
        Task<bool> Add();
        Task<bool> Update();
        Task<bool> Delete();
    }
}
