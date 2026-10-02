using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IDistrictService
    {
        Task<List<DistrictViewModel>> GetAllDistricts();
        Task<DistrictViewModel?> GetDistrictDetailsById(int districtId);
        Task<bool> Add(DistrictViewModel districtViewModel);
        Task<bool> Update(DistrictViewModel districtViewModel);
        Task<bool> Delete(int districtId);
    }
}
