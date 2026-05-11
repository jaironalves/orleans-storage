using Orleans.Storage.Application.StateHandler.TrackedCollections;

namespace Orleans.Storage.Application.Grains.Dealership.States;

[GenerateSerializer]
[Alias(nameof(DealershipCarState))]
public record DealershipCarState : IDeepCloneable<DealershipCarState>
{
    [Id(0)]
    public string? Make { get; set; }
    [Id(1)]
    public string? Model { get; set; }
    [Id(2)]
    public int Year { get; set; }    

    public DealershipCarState DeepClone()
    {
        return this;
    }
}
