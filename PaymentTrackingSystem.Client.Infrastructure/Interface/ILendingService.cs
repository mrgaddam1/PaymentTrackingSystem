using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ILendingService
    {
        Task<T?> GetAllLendings<T>();
        Task<T?> GetLendingDetailsById<T>(int lendingId);
        Task<bool> Add(LendingViewModel lending);
        Task<bool> Update(LendingViewModel lendingViewModel);
        Task<bool> Delete(int lendingId);
        Task<T?> GetPaymentModes<T>();
        Task<T?> GetAllLendingDueDateDescriptions<T>();
        Task<T?> GetAllInterestRates<T>();
    }
}
