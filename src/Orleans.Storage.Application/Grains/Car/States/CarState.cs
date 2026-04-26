namespace Orleans.Storage.Application.Grains.Car.States;

[GenerateSerializer]
[Alias(nameof(CarState))]
public class CarState
{
    [Id(0)]
    public string? Make { get; set; }
    [Id(1)]
    public string? Model { get; set; }
    [Id(2)]
    public int Year { get; set; }
}
