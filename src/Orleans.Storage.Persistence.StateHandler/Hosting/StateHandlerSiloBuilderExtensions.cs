using Microsoft.Extensions.Options;
using Orleans.Storage.Persistence.StateHandler.Options;

namespace Orleans.Storage.Persistence.StateHandler.Hosting;

public static class StateHandlerSiloBuilderExtensions
{

    /// <summary>
    /// Configures StateHandler as a grain storage provider.
    /// </summary>
    public static ISiloBuilder AddStateHandlerGrainStorage(this ISiloBuilder builder, string name, Action<StateHandlerGrainStorageOptions> configureOptions)
    {
        return builder.AddStateHandlerGrainStorage(name, optionsBuilder => optionsBuilder.Configure(configureOptions));
    }

    /// <summary>
    /// Configures StateHandler as a grain storage provider.
    /// </summary>
    public static ISiloBuilder AddStateHandlerGrainStorage(this ISiloBuilder builder, string name, Action<OptionsBuilder<StateHandlerGrainStorageOptions>>? configureOptionsBuilder = null)
    {
        builder.ConfigureServices(services => services.AddStateHandlerGrainStorage(name, configureOptionsBuilder));
        return builder;
    }
}
