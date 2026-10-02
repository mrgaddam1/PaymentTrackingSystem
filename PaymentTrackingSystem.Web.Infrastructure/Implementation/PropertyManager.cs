using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentTrackingSystem.Core.Data.Models;
using PaymentTrackingSystem.Shared;
using PaymentTrackingSystem.Web.Infrastructure.Interface;

namespace PaymentTrackingSystem.Web.Infrastructure.Implementation
{
    public class PropertyManager : IPropertyManager
    {
        private readonly ILogger<PropertyManager> logger;
        private readonly IMapper mapper;
        private PTSContext DbContext { get; set; }

        public PropertyManager(PTSContext _DbContext, IMapper _mapper)
        {
            DbContext = _DbContext;
            mapper = _mapper;
        }

        public async Task<bool> Add(PropertyViewModel propertyViewModel)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var property = new Property
                {
                    PropertyName = propertyViewModel.PropertyName,
                    PropertyTypeId = propertyViewModel.PropertyTypeId,
                    PropertOwnerName = propertyViewModel.PropertOwnerName,
                    OwnerMobileNumber = propertyViewModel.OwnerMobileNumber,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                };

                DbContext.Properties.Add(property);
                await DbContext.SaveChangesAsync();

                var propertyAddress = new PropertyAddress
                {
                    PropertyId = property.PropertyId,
                    AddressLine1 = propertyViewModel.AddressLine1,
                    AddressLine2 = propertyViewModel.AddressLine2,
                    Postcode = propertyViewModel.Postcode,
                    DistrictId = propertyViewModel.DistrictId,
                    StateId = propertyViewModel.StateId,
                    CountryId = propertyViewModel.CountryId,
                };

                DbContext.PropertyAddresses.Add(propertyAddress);
                await DbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while processing the request.");
                return false;
            }
        }

        public async Task<bool> Delete(int propertyId)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var property = await DbContext.Properties.FindAsync(propertyId);
                if (property == null)
                {
                    return false;
                }

                property.DeleteDate = DateTime.Now;
                DbContext.Properties.Update(property);
                await DbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while processing the request.");
                return false;
            }
        }

        public async Task<List<PropertyViewModel>> GetAllProperties()
        {
            var propertyData = new List<PropertyViewModel>();
            try
            {
                propertyData = await (from p in DbContext.Properties
                                      join pa in DbContext.PropertyAddresses
                                      on p.PropertyId equals pa.PropertyId into addressGroup
                                      from pa in addressGroup.DefaultIfEmpty()
                                      where p.DeleteDate == null
                                      select new PropertyViewModel
                                      {
                                          PropertyId = p.PropertyId,
                                          PropertyName = p.PropertyName,
                                          PropertyTypeId = p.PropertyTypeId,
                                          PropertOwnerName = p.PropertOwnerName,
                                          OwnerMobileNumber = p.OwnerMobileNumber,
                                          AddressLine1 = pa != null ? pa.AddressLine1 : null,
                                          AddressLine2 = pa != null ? pa.AddressLine2 : null,
                                          Postcode = pa != null ? pa.Postcode : null,
                                          DistrictId = pa != null ? pa.DistrictId : null,
                                          StateId = pa != null ? pa.StateId : null,
                                          CountryId = pa != null ? pa.CountryId : null,
                                      }).OrderBy(x => x.PropertyName).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing the request.");
                propertyData = new List<PropertyViewModel>();
            }
            return propertyData;
        }

        public async Task<PropertyViewModel> GetPropertyDetailsById(int propertyId)
        {
            try
            {
                var property = await (from p in DbContext.Properties
                                      join pa in DbContext.PropertyAddresses
                                      on p.PropertyId equals pa.PropertyId into addressGroup
                                      from pa in addressGroup.DefaultIfEmpty()
                                      where p.PropertyId == propertyId && p.DeleteDate == null
                                      select new PropertyViewModel
                                      {
                                          PropertyId = p.PropertyId,
                                          PropertyName = p.PropertyName,
                                          PropertyTypeId = p.PropertyTypeId,
                                          PropertOwnerName = p.PropertOwnerName,
                                          OwnerMobileNumber = p.OwnerMobileNumber,
                                          AddressLine1 = pa != null ? pa.AddressLine1 : null,
                                          AddressLine2 = pa != null ? pa.AddressLine2 : null,
                                          Postcode = pa != null ? pa.Postcode : null,
                                          DistrictId = pa != null ? pa.DistrictId : null,
                                          StateId = pa != null ? pa.StateId : null,
                                          CountryId = pa != null ? pa.CountryId : null,
                                      }).FirstOrDefaultAsync();

                return property ?? new PropertyViewModel();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "An error occurred while processing the request.");
                return new PropertyViewModel();
            }
        }

        public async Task<bool> Update(PropertyViewModel propertyViewModel)
        {
            using var transaction = await DbContext.Database.BeginTransactionAsync();
            try
            {
                var property = await DbContext.Properties.FindAsync(propertyViewModel.PropertyId);
                if (property == null)
                {
                    return false;
                }

                property.PropertyName = propertyViewModel.PropertyName;
                property.PropertyTypeId = propertyViewModel.PropertyTypeId;
                property.PropertOwnerName = propertyViewModel.PropertOwnerName;
                property.OwnerMobileNumber = propertyViewModel.OwnerMobileNumber;
                property.ModifiedDate = DateTime.Now;

                DbContext.Properties.Update(property);
                await DbContext.SaveChangesAsync();

                var propertyAddress = await DbContext.PropertyAddresses
                    .FirstOrDefaultAsync(pa => pa.PropertyId == propertyViewModel.PropertyId);

                if (propertyAddress != null)
                {
                    propertyAddress.AddressLine1 = propertyViewModel.AddressLine1;
                    propertyAddress.AddressLine2 = propertyViewModel.AddressLine2;
                    propertyAddress.Postcode = propertyViewModel.Postcode;
                    propertyAddress.DistrictId = propertyViewModel.DistrictId;
                    propertyAddress.StateId = propertyViewModel.StateId;
                    propertyAddress.CountryId = propertyViewModel.CountryId;

                    DbContext.PropertyAddresses.Update(propertyAddress);
                    await DbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex.Message, "An error occurred while processing the request.");
                return false;
            }
        }
    }
}
