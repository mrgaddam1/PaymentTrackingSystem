using PaymentTrackingSystem.Shared;

namespace PaymentTrackingSystem.Client.Infrastructure.Interface
{
    public interface IStateService
    {
        Task<List<StateViewModel>> GetAllStates();
        Task<StateViewModel?> GetStateDetailsById(int stateId);
        Task<bool> Add(StateViewModel stateViewModel);
        Task<bool> Update(StateViewModel stateViewModel);
        Task<bool> Delete(int stateId);
    }
}
