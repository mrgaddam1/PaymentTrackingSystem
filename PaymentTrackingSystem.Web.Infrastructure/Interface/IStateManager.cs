using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Web.Infrastructure.Interface
{
    public interface IStateManager
    {
        Task<List<StateViewModel>> GetAllStates();
        Task<StateViewModel?> GetStateDetailsById(int stateId);
        Task<bool> Add(StateViewModel stateViewModel);
        Task<bool> Update(StateViewModel stateViewModel);
        Task<bool> Delete(int stateId);
    }
}
