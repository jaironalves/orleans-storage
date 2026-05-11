using Orleans.Storage.Application.Grains.Base;
using Orleans.Storage.Application.Grains.Dealership.States;

namespace Orleans.Storage.Application.Grains.Dealership;

internal class DealershipGrain(
    [PersistentState("dealership", "state-handler-storage")]
    IPersistentState<DealershipState> dealershipState) : BaseGrain, IDealershipGrain
{
    public async Task<DealershipState?> GetStateAsync()
    {
        await Task.Delay(100); // Simulate some asynchronous work
        var state = dealershipState.State;
        return state;
    }
}