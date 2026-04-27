using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using Orleans.Runtime.Hosting;
using Orleans.Storage.Persistence.StateHandler.Abstractions;
using Orleans.Storage.Persistence.StateHandler.Options;
using Orleans.Storage.Persistence.StateHandler.Storage;

namespace Orleans.Storage.Persistence.StateHandler.Hosting;

internal static class StateHandlerServiceCollectionExtensions
{
    /// <summary>
    /// Configures StateHandler as a grain storage provider.
    /// </summary>
    internal static IServiceCollection AddStateHandlerGrainStorage(this IServiceCollection services, string name, Action<OptionsBuilder<StateHandlerGrainStorageOptions>>? configureOptions = null)
    {
        configureOptions?.Invoke(services.AddOptions<StateHandlerGrainStorageOptions>(name));
        services.ConfigureNamedOptionForLogging<StateHandlerGrainStorageOptions>(name);

        services.AddKeyedSingleton<IStateHandlerFactory>(name, (sp, key) =>
        {
            var options = sp
                .GetRequiredService<IOptionsMonitor<StateHandlerGrainStorageOptions>>()
                .Get(name);

            return new StateHandlerFactory(sp, name, options);
        });       

        return services.AddGrainStorage(name, StateHandlerGrainStorageFactory.Create);
    }
}
