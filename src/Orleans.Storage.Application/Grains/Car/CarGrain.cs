using Orleans.Storage.Application.Grains.Car.States;
using System.Threading.Tasks;
using Orleans.Streaming;
using Orleans.Storage.Application.Grains.Base;

namespace Orleans.Storage.Application.Grains.Car;

public class CarGrain(
    [PersistentState("car", "state-handler-storage")]
    IPersistentState<CarState> carState) : BaseGrain, ICarGrain
{
    public async Task<CarState?> GetStateAsync()
    {
        await Task.Delay(100); // Simulate some asynchronous work
        var state = carState.State;
        return state;
    }
}
