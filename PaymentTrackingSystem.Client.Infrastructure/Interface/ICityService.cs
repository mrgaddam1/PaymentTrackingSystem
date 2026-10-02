using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ICityService
    {
        Task<List<CityViewModel>> GetAllCities();
        Task<CityViewModel?> GetCityDetailsById(int cityId);
        Task<bool> Add(CityViewModel cityViewModel);
        Task<bool> Update(CityViewModel cityViewModel);
        Task<bool> Delete(int cityId);
    }
}
