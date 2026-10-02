using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface ICityManager
    {
        Task<List<CityViewModel>> GetAllCities();
        Task<CityViewModel?> GetCityDetailsById(int cityId);
        Task<bool> Add(CityViewModel cityViewModel);
        Task<bool> Update(CityViewModel cityViewModel);
        Task<bool> Delete(int cityId);
    }
}
