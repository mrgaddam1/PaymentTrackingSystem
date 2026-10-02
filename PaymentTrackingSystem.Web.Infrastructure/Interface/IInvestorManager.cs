using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IInvestorManager
    {
        Task<List<InvestorViewModel>> GetAllInvestors();
        Task<InvestorViewModel> GetInvestorDetailsById(int investorId);
        Task<bool> Add(InvestorViewModel investor);
        Task<bool> Update(InvestorViewModel investorViewModel);
        Task<bool> Delete(int investorId);
    }
}
