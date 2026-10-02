using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class DistrictManager : IDistrictManager
    {
        private readonly PTSContext dbContext;
        private readonly ILogger<DistrictManager> logger;

        public DistrictManager(PTSContext dbContext, ILogger<DistrictManager> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<DistrictViewModel>> GetAllDistricts()
        {
            return await dbContext.Districts
                .AsNoTracking()
                .OrderBy(district => district.DistrictName)
                .Select(district => new DistrictViewModel
                {
                    DistrictId = district.DistrictId,
                    DistrictName = district.DistrictName
                })
                .ToListAsync();
        }

        public async Task<DistrictViewModel?> GetDistrictDetailsById(int districtId)
        {
            var district = await dbContext.Districts
                .AsNoTracking()
                .Where(item => item.DistrictId == districtId)
                .Select(item => new DistrictViewModel
                {
                    DistrictId = item.DistrictId,
                    DistrictName = item.DistrictName
                })
                .FirstOrDefaultAsync();

            return district;
        }

        public async Task<bool> Add(DistrictViewModel districtViewModel)
        {
            if (string.IsNullOrWhiteSpace(districtViewModel.DistrictName))
            {
                return false;
            }

            try
            {
                dbContext.Districts.Add(new District
                {
                    DistrictName = districtViewModel.DistrictName.Trim()
                });

                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add district.");
                return false;
            }
        }

        public async Task<bool> Update(DistrictViewModel districtViewModel)
        {
            if (districtViewModel.DistrictId <= 0 || string.IsNullOrWhiteSpace(districtViewModel.DistrictName))
            {
                return false;
            }

            try
            {
                var district = await dbContext.Districts.FindAsync(districtViewModel.DistrictId);
                if (district == null)
                {
                    return false;
                }

                district.DistrictName = districtViewModel.DistrictName.Trim();
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update district {DistrictId}.", districtViewModel.DistrictId);
                return false;
            }
        }

        public async Task<bool> Delete(int districtId)
        {
            try
            {
                var district = await dbContext.Districts.FindAsync(districtId);
                if (district == null)
                {
                    return false;
                }

                dbContext.Districts.Remove(district);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete district {DistrictId}.", districtId);
                return false;
            }
        }
    }
}
