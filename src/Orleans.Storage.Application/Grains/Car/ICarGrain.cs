using System.Threading.Tasks;
using Orleans.Storage.Application.Grains.Car.States;

namespace Orleans.Storage.Application.Grains.Car;

[Alias(nameof(ICarGrain))]
public interface ICarGrain : IGrainWithStringKey
{
    [Alias(nameof(GetStateAsync))]
    Task<CarState?> GetStateAsync();
}
