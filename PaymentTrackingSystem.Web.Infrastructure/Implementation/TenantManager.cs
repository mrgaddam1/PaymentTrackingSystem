using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class TenantManager : ITenantManager
    {
        private readonly ILogger<TenantManager> logger;
        private PTSContext DbContext { get; }

        public TenantManager(PTSContext dbContext, ILogger<TenantManager> logger)
        {
            DbContext = dbContext;
            this.logger = logger;
        }

        public async Task<bool> Add(TenantViewModel tenantViewModel)
        {
            await using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var tenant = new Tenant
                {
                    UserId = 1,
                    FirstName = tenantViewModel.FirstName,
                    LastName = tenantViewModel.LastName,
                    MobileNumber = tenantViewModel.MobileNumber,
                    EmailId = tenantViewModel.EmailId,
                    TenantTypeId = tenantViewModel.TenantTypeId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                DbContext.Tenants.Add(tenant);
                await DbContext.SaveChangesAsync();
                AddAddresses(tenant.TenantId, tenantViewModel);
                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while adding tenant.");
                return false;
            }
        }

        public async Task<bool> Update(TenantViewModel tenantViewModel)
        {
            await using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var tenant = await DbContext.Tenants.FirstOrDefaultAsync(x => x.TenantId == tenantViewModel.TenantId && x.DeletedDate == null);
                if (tenant == null)
                {
                    return false;
                }

                tenant.FirstName = tenantViewModel.FirstName;
                tenant.LastName = tenantViewModel.LastName;
                tenant.MobileNumber = tenantViewModel.MobileNumber;
                tenant.EmailId = tenantViewModel.EmailId;
                tenant.TenantTypeId = tenantViewModel.TenantTypeId;
                tenant.ModifiedDate = DateTime.UtcNow;

                var address = await DbContext.TenantAddresses.FirstOrDefaultAsync(x => x.TenantId == tenant.TenantId && x.DeletedDate == null);
                if (address == null)
                {
                    AddCurrentAddress(tenant.TenantId, tenantViewModel);
                }
                else
                {
                    UpdateCurrentAddress(address, tenantViewModel);
                }

                var previousAddress = await DbContext.TenantPreviousAddresses.FirstOrDefaultAsync(x => x.TenantId == tenant.TenantId && x.DeletedDate == null);
                if (tenantViewModel.DoesHavePreviousAddress)
                {
                    if (previousAddress == null)
                    {
                        AddPreviousAddress(tenant.TenantId, tenantViewModel);
                    }
                    else
                    {
                        UpdatePreviousAddress(previousAddress, tenantViewModel);
                    }
                }
                else if (previousAddress != null)
                {
                    previousAddress.DeletedDate = DateTime.UtcNow;
                    previousAddress.ModifiedDate = DateTime.UtcNow;
                }

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while updating tenant.");
                return false;
            }
        }

        public async Task<bool> Delete(int tenantId)
        {
            await using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var tenant = await DbContext.Tenants.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DeletedDate == null);
                if (tenant == null)
                {
                    return false;
                }

                var deletedDate = DateTime.UtcNow;
                tenant.DeletedDate = deletedDate;
                tenant.ModifiedDate = deletedDate;
                tenant.IsActive = false;

                var addresses = await DbContext.TenantAddresses.Where(x => x.TenantId == tenantId && x.DeletedDate == null).ToListAsync();
                foreach (var address in addresses)
                {
                    address.DeletedDate = deletedDate;
                    address.ModifiedDate = deletedDate;
                }

                var previousAddresses = await DbContext.TenantPreviousAddresses.Where(x => x.TenantId == tenantId && x.DeletedDate == null).ToListAsync();
                foreach (var address in previousAddresses)
                {
                    address.DeletedDate = deletedDate;
                    address.ModifiedDate = deletedDate;
                }

                await DbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while deleting tenant.");
                return false;
            }
        }

        public async Task<List<TenantViewModel>> GetAllTenants()
        {
            var records = await (from tenant in DbContext.Tenants.AsNoTracking()
                                 join address in DbContext.TenantAddresses.AsNoTracking() on tenant.TenantId equals address.TenantId into addressGroup
                                 from address in addressGroup.Where(x => x.DeletedDate == null).DefaultIfEmpty()
                                 where tenant.DeletedDate == null
                                 orderby tenant.FirstName, tenant.LastName
                                 select new { tenant, address }).ToListAsync();

            return records.Select(x => ToViewModel(x.tenant, x.address, null)).ToList();
        }

        public async Task<TenantViewModel?> GetTenantDetailsById(int tenantId)
        {
            var tenant = await DbContext.Tenants.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DeletedDate == null);
            if (tenant == null)
            {
                return null;
            }

            var address = await DbContext.TenantAddresses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DeletedDate == null);
            var previousAddress = await DbContext.TenantPreviousAddresses.AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.DeletedDate == null);

            return ToViewModel(tenant, address, previousAddress);
        }

        private static TenantViewModel ToViewModel(Tenant tenant, TenantAddress? address, TenantPreviousAddress? previousAddress)
        {
            return new TenantViewModel
            {
                TenantId = tenant.TenantId,
                FirstName = tenant.FirstName,
                LastName = tenant.LastName,
                MobileNumber = tenant.MobileNumber,
                EmailId = tenant.EmailId,
                TenantTypeId = tenant.TenantTypeId,
                AddressLine1 = address == null ? null : address.AddressLine1,
                AddressLine2 = address == null ? null : address.AddressLine2,
                AddressLine3 = address == null ? null : address.AddressLine3,
                Postcode = address == null ? null : address.Postcode,
                CityId = address == null ? null : address.CityId,
                StateId = address == null ? null : address.StateId,
                CountryId = address == null ? null : address.CountryId,
                DoesHavePreviousAddress = previousAddress != null,
                PreviousAddressLine1 = previousAddress == null ? null : previousAddress.AddressLine1,
                PreviousAddressLine2 = previousAddress == null ? null : previousAddress.AddressLine2,
                PreviousAddressLine3 = previousAddress == null ? null : previousAddress.AddressLine3,
                PreviousPostcode = previousAddress == null ? null : previousAddress.Postcode,
                PreviousCityId = previousAddress == null ? null : previousAddress.CityId,
                PreviousStateId = previousAddress == null ? null : previousAddress.StateId
            };
        }

        private void AddAddresses(int tenantId, TenantViewModel model)
        {
            AddCurrentAddress(tenantId, model);
            if (model.DoesHavePreviousAddress)
            {
                AddPreviousAddress(tenantId, model);
            }
        }

        private void AddCurrentAddress(int tenantId, TenantViewModel model)
        {
            DbContext.TenantAddresses.Add(new TenantAddress
            {
                UserId = 1,
                TenantId = tenantId,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                Postcode = model.Postcode,
                CityId = model.CityId,
                StateId = model.StateId,
                CountryId = model.CountryId,
                CreatedDate = DateTime.UtcNow
            });
        }

        private void UpdateCurrentAddress(TenantAddress address, TenantViewModel model)
        {
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.AddressLine3 = model.AddressLine3;
            address.Postcode = model.Postcode;
            address.CityId = model.CityId;
            address.StateId = model.StateId;
            address.CountryId = model.CountryId;
            address.ModifiedDate = DateTime.UtcNow;
        }

        private void AddPreviousAddress(int tenantId, TenantViewModel model)
        {
            DbContext.TenantPreviousAddresses.Add(new TenantPreviousAddress
            {
                UserId = 1,
                TenantId = tenantId,
                AddressLine1 = model.PreviousAddressLine1,
                AddressLine2 = model.PreviousAddressLine2,
                AddressLine3 = model.PreviousAddressLine3,
                Postcode = model.PreviousPostcode,
                CityId = model.PreviousCityId,
                StateId = model.PreviousStateId,
                CreatedDate = DateTime.UtcNow
            });
        }

        private void UpdatePreviousAddress(TenantPreviousAddress address, TenantViewModel model)
        {
            address.AddressLine1 = model.PreviousAddressLine1;
            address.AddressLine2 = model.PreviousAddressLine2;
            address.AddressLine3 = model.PreviousAddressLine3;
            address.Postcode = model.PreviousPostcode;
            address.CityId = model.PreviousCityId;
            address.StateId = model.PreviousStateId;
            address.ModifiedDate = DateTime.UtcNow;
        }
    }
}
