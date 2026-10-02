using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IDistrictManager
    {
        Task<List<DistrictViewModel>> GetAllDistricts();
        Task<DistrictViewModel?> GetDistrictDetailsById(int districtId);
        Task<bool> Add(DistrictViewModel districtViewModel);
        Task<bool> Update(DistrictViewModel districtViewModel);
        Task<bool> Delete(int districtId);
    }
}
