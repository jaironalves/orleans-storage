using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime.Hosting;
using Orleans.Storage.Persistence.StateHandler.Storage;

namespace Orleans.Storage.Persistence.StateHandler.Extensions;

internal static class StateHandlerServiceCollectionExtensions
{
    /// <summary>
    /// Configures StateHandler as a grain storage provider.
    /// </summary>
    public static IServiceCollection AddStateHandlerGrainStorage(this IServiceCollection services, string name)
    {        
        return services.AddGrainStorage(name, StateHandlerGrainStorageFactory.Create);
    }
}
