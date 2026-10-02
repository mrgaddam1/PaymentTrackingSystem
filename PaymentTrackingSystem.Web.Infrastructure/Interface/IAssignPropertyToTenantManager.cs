using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IAssignPropertyToTenantManager
    {
        Task<List<AssignPropertyToTenantViewModel>> GetAllAssignments();
        Task<AssignPropertyToTenantViewModel?> GetAssignmentById(int rentId);
        Task<bool> Add(AssignPropertyToTenantViewModel assignment);
        Task<bool> Update(AssignPropertyToTenantViewModel assignment);
        Task<bool> Delete(int rentId);
    }
}
