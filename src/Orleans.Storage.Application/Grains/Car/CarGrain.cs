using Orleans.Storage.Application.Grains.Car.States;
using System.Threading.Tasks;

namespace Orleans.Storage.Application.Grains.Car;

public class CarGrain(
    [PersistentState("car", "state-handler-storage")]
    IPersistentState<CarState> carState) : ICarGrain
{
    public Task<CarState?> GetStateAsync()
    {
        return Task.FromResult<CarState?>(carState.State);
    }
}
