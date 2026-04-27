using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Storage.Persistence.StateHandler.Abstractions;
using Orleans.Storage.Persistence.StateHandler.Options;

namespace Orleans.Storage.Persistence.StateHandler.Hosting;

public class StateHandlerSiloBuilder(ISiloBuilder siloBuilder, string name) :
    ISiloBuilder
{
    public IServiceCollection Services => siloBuilder.Services;

    public IConfiguration Configuration => siloBuilder.Configuration;

    public StateHandlerSiloBuilder AddStateHandler<TState, THandler>()
        where THandler : class, IStateHandler<TState>
    {
        Services.Configure<StateHandlerGrainStorageOptions>(name, options =>
        {
            options.Handlers[typeof(TState)] = typeof(THandler);
        });

        Services.TryAddScoped<IStateHandler<TState>, THandler>();

        return this;
    }
}
