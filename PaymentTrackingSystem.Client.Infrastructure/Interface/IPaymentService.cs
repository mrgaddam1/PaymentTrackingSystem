using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IPaymentService
    {
        Task<T?> GetAllClientPayments<T>();
        Task<T?> GetAllClientPaymentsDetailsByPaymentId<T>(int paymentId);
        Task<bool> Add(ClientPaymentViewModel clientPayment);
        Task<bool> Update(ClientPaymentViewModel clientPaymentViewModel);
        Task<bool> Delete(ClientPaymentViewModel clientPaymentViewModel);
    }
}
