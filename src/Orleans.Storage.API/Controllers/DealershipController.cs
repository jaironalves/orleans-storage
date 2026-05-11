using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Storage.Application.Grains.Dealership;
using Orleans.Storage.Application.Grains.Dealership.States;

namespace Orleans.Storage.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DealershipController(IGrainFactory grainFactory) : ControllerBase
{
    private readonly IGrainFactory _grainFactory = grainFactory;

    [HttpGet("{id}")]
    public async Task<ActionResult<DealershipState?>> Get(string id)
    {
        var grain = _grainFactory.GetGrain<IDealershipGrain>(id);
        var state = await grain.GetStateAsync();
        if (state == null)
            return NotFound();
        return Ok(state);
    }
}
