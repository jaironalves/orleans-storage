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

    public Task InitAsync()
    {
        dealershipState.State.Location = "Location";
        dealershipState.State.City = "New York";
        dealershipState.State.Cars.Add("Car1", new DealershipCarState { Model = "Model S", Make = "Tesla", Year = 2025 });
        return dealershipState.WriteStateAsync();        
    }
}