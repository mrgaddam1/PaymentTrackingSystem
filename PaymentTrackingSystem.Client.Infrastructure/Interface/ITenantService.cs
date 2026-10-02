using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface ITenantService
    {
        Task<List<TenantViewModel>> GetAllTenants();
        Task<TenantViewModel?> GetTenantDetailsById(int tenantId);
        Task<bool> Add(TenantViewModel tenantViewModel);
        Task<bool> Update(TenantViewModel tenantViewModel);
        Task<bool> Delete(int tenantId);
    }
}
