using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IPropertyTypeManager
    {
        Task<List<PropertyTypeViewModel>> GetAllPropertyTypes();
        Task<PropertyTypeViewModel?> GetPropertyTypeDetailsById(int propertyTypeId);
        Task<bool> Add(PropertyTypeViewModel propertyTypeViewModel);
        Task<bool> Update(PropertyTypeViewModel propertyTypeViewModel);
        Task<bool> Delete(int propertyTypeId);
    }
}
