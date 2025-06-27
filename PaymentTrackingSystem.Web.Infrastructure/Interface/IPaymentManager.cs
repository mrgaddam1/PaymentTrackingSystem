using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IPaymentManager
    {
        Task<List<ClientPaymentViewModel>> GetAllClientPayments();
        Task<ClientPaymentViewModel> GetClientPaymentDetailsById(int paymentId);
        Task<bool> Add(ClientPaymentViewModel clientPayment);
        Task<bool> Update(ClientPaymentViewModel clientPaymentViewModel);
        Task<bool> Delete(int paymentId);
    }
}
