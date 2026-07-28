using Orleans.Storage.Application.Grains.Dealership.States;

namespace Orleans.Storage.Application.Grains.Dealership;

[Alias(nameof(IDealershipGrain))]
public interface IDealershipGrain : IGrainWithStringKey
{
    [Alias(nameof(GetStateAsync))]
    Task<DealershipState?> GetStateAsync();

    [Alias(nameof(InitAsync))]
    Task InitAsync();
}
