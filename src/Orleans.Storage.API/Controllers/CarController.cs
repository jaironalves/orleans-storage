using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Storage.Application.Grains.Car;
using Orleans.Storage.Application.Grains.Car.States;

namespace Orleans.Storage.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarController(IGrainFactory grainFactory) : ControllerBase
{
    private readonly IGrainFactory _grainFactory = grainFactory;

    [HttpGet("{id}")]
    public async Task<ActionResult<CarState?>> Get(string id)
    {
        var grain = _grainFactory.GetGrain<ICarGrain>(id);
        var state = await grain.GetStateAsync();
        if (state == null)
            return NotFound();
        return Ok(state);
    }
}
