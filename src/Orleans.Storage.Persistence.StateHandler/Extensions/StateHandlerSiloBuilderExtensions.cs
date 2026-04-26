namespace Orleans.Storage.Persistence.StateHandler.Extensions;

public static class StateHandlerSiloBuilderExtensions
{
    /// <summary>
    /// Configures StateHandler as a grain storage provider.
    /// </summary>
    public static ISiloBuilder AddStateHandlerGrainStorage(this ISiloBuilder builder, string name)
    {
        return builder.ConfigureServices(services => services.AddStateHandlerGrainStorage(name));
    }
}
