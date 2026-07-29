using Orleans.Storage.Application.StateHandler.TrackedCollections;

namespace Orleans.Storage.Application.Grains.Dealership.States;

[GenerateSerializer]
[Alias(nameof(DealershipCarState))]
public record DealershipCarState : ITrackedValue<DealershipCarState>
{
    [Id(0)]
    public string? Id { get; set; }
    [Id(1)]
    public string? Make { get; set; }
    [Id(2)]
    public string? Model { get; set; }
    [Id(3)]
    public int Year { get; set; }    

    public DealershipCarState DeepClone()
    {
        return this;
    }
}
