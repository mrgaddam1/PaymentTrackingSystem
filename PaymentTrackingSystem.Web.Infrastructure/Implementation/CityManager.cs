using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class CityManager : ICityManager
    {
        private readonly PTSContext dbContext;
        private readonly ILogger<CityManager> logger;

        public CityManager(PTSContext dbContext, ILogger<CityManager> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<CityViewModel>> GetAllCities()
        {
            return await dbContext.Citys
                .AsNoTracking()
                .OrderBy(city => city.CityName)
                .Select(city => new CityViewModel
                {
                    CityId = city.CityId,
                    CityName = city.CityName
                })
                .ToListAsync();
        }

        public async Task<CityViewModel?> GetCityDetailsById(int cityId)
        {
            return await dbContext.Citys
                .AsNoTracking()
                .Where(city => city.CityId == cityId)
                .Select(city => new CityViewModel
                {
                    CityId = city.CityId,
                    CityName = city.CityName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Add(CityViewModel cityViewModel)
        {
            if (string.IsNullOrWhiteSpace(cityViewModel.CityName))
            {
                return false;
            }

            try
            {
                dbContext.Citys.Add(new City { CityName = cityViewModel.CityName.Trim() });
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add city.");
                return false;
            }
        }

        public async Task<bool> Update(CityViewModel cityViewModel)
        {
            if (cityViewModel.CityId <= 0 || string.IsNullOrWhiteSpace(cityViewModel.CityName))
            {
                return false;
            }

            try
            {
                var city = await dbContext.Citys.FindAsync(cityViewModel.CityId);
                if (city == null)
                {
                    return false;
                }

                city.CityName = cityViewModel.CityName.Trim();
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update city {CityId}.", cityViewModel.CityId);
                return false;
            }
        }

        public async Task<bool> Delete(int cityId)
        {
            try
            {
                var city = await dbContext.Citys.FindAsync(cityId);
                if (city == null)
                {
                    return false;
                }

                dbContext.Citys.Remove(city);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete city {CityId}.", cityId);
                return false;
            }
        }
    }
}
