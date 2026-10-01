using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IInvestorService
    {
        Task<T?> GetAllInvestors<T>();
        Task<T?> GetInvestorDetailsById<T>(int investorId);
        Task<bool> Add(InvestorViewModel investor);
        Task<bool> Update(InvestorViewModel investorViewModel);
        Task<bool> Delete(int investorId);
    }
}
