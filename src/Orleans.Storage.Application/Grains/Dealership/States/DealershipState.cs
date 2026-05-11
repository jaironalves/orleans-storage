using Orleans.Storage.Application.StateHandler.TrackedCollections;

namespace Orleans.Storage.Application.Grains.Dealership.States;

[GenerateSerializer]
[Alias(nameof(DealershipState))]
public class DealershipState
{
    [Id(0)]
    public string? Name { get; set; }
    [Id(1)]
    public string? Location { get; set; }
    [Id(2)]
    public string? City { get; set; }

    [Id(3)]
    public TrackedDictionary<string, DealershipCarState> Cars { get; set; } = [];
}
