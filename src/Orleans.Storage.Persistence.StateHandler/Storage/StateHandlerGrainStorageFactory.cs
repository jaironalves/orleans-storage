using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Configuration;

namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal static class StateHandlerGrainStorageFactory
{
    public static StateHandlerGrainStorage Create(IServiceProvider services, string name)
    {
        var clusterOptions = services.GetRequiredService<IOptions<ClusterOptions>>().Value;
        return ActivatorUtilities.CreateInstance<StateHandlerGrainStorage>(services, name, clusterOptions);
    }
}
