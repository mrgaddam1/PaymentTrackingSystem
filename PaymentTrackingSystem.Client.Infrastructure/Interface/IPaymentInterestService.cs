using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IPaymentInterestService
    {
        Task<T?> GetAllClientPaymentInterests<T>();
        Task<T?> GetClientPaymentInterestsDetailsById<T>(int paymentId);
        Task<bool> Add(ClientPaymentInterestViewModel clientPaymentInterestViewModel);
        Task<bool> Update(ClientPaymentInterestViewModel clientPaymentInterestViewModel);
        Task<bool> Delete(int paymentId);
    }
}
