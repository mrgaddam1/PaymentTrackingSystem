using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IAssignPropertyToTenantService
    {
        Task<List<AssignPropertyToTenantViewModel>> GetAllAssignments();
        Task<AssignPropertyToTenantViewModel?> GetAssignmentById(int rentId);
        Task<bool> Add(AssignPropertyToTenantViewModel assignment);
        Task<bool> Update(AssignPropertyToTenantViewModel assignment);
        Task<bool> Delete(int rentId);
    }
}
