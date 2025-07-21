using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IPaymentInterestManager
    {
        Task<List<ClientPaymentInterestViewModel>> GetAll();
        Task<ClientPaymentInterestViewModel> GetAllDetailsById(int interestId);
        Task<string> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel);
        Task<bool> Update(ClientPaymentInterestViewModel clientPaymentInterestViewModel);
        Task<bool> Delete(int interestId);
        Task<List<ClientPaymentInterestPending>> GetAllClientsPaymentInterestsPendingDetais();

    }
}
