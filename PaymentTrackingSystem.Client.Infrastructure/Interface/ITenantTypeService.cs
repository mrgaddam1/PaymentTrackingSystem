using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ITenantTypeService
    {
        Task<List<TenantTypeViewModel>> GetAllTenantTypes();
        Task<TenantTypeViewModel?> GetTenantTypeDetailsById(int tenantTypeId);
        Task<bool> Add(TenantTypeViewModel tenantTypeViewModel);
        Task<bool> Update(TenantTypeViewModel tenantTypeViewModel);
        Task<bool> Delete(int tenantTypeId);
    }
}
