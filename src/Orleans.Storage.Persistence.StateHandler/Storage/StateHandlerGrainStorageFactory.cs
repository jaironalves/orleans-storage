using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Storage.Persistence.StateHandler.Abstractions;

namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal static class StateHandlerGrainStorageFactory
{
    public static StateHandlerGrainStorage Create(IServiceProvider services, string name)
    {
        var clusterOptions = services.GetRequiredService<IOptions<ClusterOptions>>().Value;
        var handlerFactory = services.GetRequiredKeyedService<IStateHandlerFactory>(name);
        return ActivatorUtilities.CreateInstance<StateHandlerGrainStorage>(services, name, clusterOptions, handlerFactory);
    }
}
