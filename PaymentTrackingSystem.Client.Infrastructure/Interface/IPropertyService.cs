using PaymentTrackingSystem.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IPropertyService
    {
        Task<List<PropertyViewModel>> GetAllProperties();
        Task<PropertyViewModel> GetPropertyDetailsById(int propertyId);
        Task<bool> Add(PropertyViewModel propertyViewModel);
        Task<bool> Update(PropertyViewModel propertyViewModel);
        Task<bool> Delete(int propertyId);
    }
}
