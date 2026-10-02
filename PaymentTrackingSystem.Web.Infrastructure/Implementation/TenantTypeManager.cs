using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class TenantTypeManager : ITenantTypeManager
    {
        private readonly PTSContext dbContext;
        private readonly ILogger<TenantTypeManager> logger;

        public TenantTypeManager(PTSContext dbContext, ILogger<TenantTypeManager> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<List<TenantTypeViewModel>> GetAllTenantTypes()
        {
            return await dbContext.TenantTypes
                .AsNoTracking()
                .OrderBy(tenantType => tenantType.TenantTypeDescription)
                .Select(tenantType => new TenantTypeViewModel
                {
                    TenantTypeId = tenantType.TenantTypeId,
                    TenantTypeName = tenantType.TenantTypeDescription
                })
                .ToListAsync();
        }

        public async Task<TenantTypeViewModel?> GetTenantTypeDetailsById(int tenantTypeId)
        {
            return await dbContext.TenantTypes
                .AsNoTracking()
                .Where(tenantType => tenantType.TenantTypeId == tenantTypeId)
                .Select(tenantType => new TenantTypeViewModel
                {
                    TenantTypeId = tenantType.TenantTypeId,
                    TenantTypeName = tenantType.TenantTypeDescription
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Add(TenantTypeViewModel tenantTypeViewModel)
        {
            if (string.IsNullOrWhiteSpace(tenantTypeViewModel.TenantTypeName))
            {
                return false;
            }

            try
            {
                dbContext.TenantTypes.Add(new TenantType
                {
                    TenantTypeDescription = tenantTypeViewModel.TenantTypeName.Trim()
                });

                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to add tenant type.");
                return false;
            }
        }

        public async Task<bool> Update(TenantTypeViewModel tenantTypeViewModel)
        {
            if (tenantTypeViewModel.TenantTypeId <= 0 || string.IsNullOrWhiteSpace(tenantTypeViewModel.TenantTypeName))
            {
                return false;
            }

            try
            {
                var tenantType = await dbContext.TenantTypes.FindAsync(tenantTypeViewModel.TenantTypeId);
                if (tenantType == null)
                {
                    return false;
                }

                tenantType.TenantTypeDescription = tenantTypeViewModel.TenantTypeName.Trim();
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to update tenant type {TenantTypeId}.", tenantTypeViewModel.TenantTypeId);
                return false;
            }
        }

        public async Task<bool> Delete(int tenantTypeId)
        {
            try
            {
                var tenantType = await dbContext.TenantTypes.FindAsync(tenantTypeId);
                if (tenantType == null)
                {
                    return false;
                }

                dbContext.TenantTypes.Remove(tenantType);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to delete tenant type {TenantTypeId}.", tenantTypeId);
                return false;
            }
        }
    }
}
