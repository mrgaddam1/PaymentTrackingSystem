using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface ILendingManager
    {
        Task<List<LendingViewModel>> GetAllLendings();
        Task<LendingViewModel> GetLendingDetailsById(int lendingId);
        Task<bool> Add(LendingViewModel lending);
        Task<bool> Update(LendingViewModel lendingViewModel);
        Task<bool> Delete(int lendingId);
        Task<List<PaymentModeViewModel>> GetPaymentModes();
        Task<List<LendingDueDateDescriptionViewModel>> GetAllLendingDueDateDescriptions();
        Task<List<InterestRatesViewModel>> GetAllInterestRates();

    }
}
