using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Serialization.Serializers;
using Orleans.Storage.Persistence.StateHandler.Abstractions;

namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal class StateHandlerGrainStorage(
    string name,
    ClusterOptions clusterOptions,
    IStateHandlerFactory handlerFactory,    
    ILogger<StateHandlerGrainStorage> logger) : IGrainStorage, ILifecycleParticipant<ISiloLifecycle>
{
    public void Participate(ISiloLifecycle lifecycle)
    {
        var lifecycleName = OptionFormattingUtilities.Name<StateHandlerGrainStorage>(name);
        lifecycle.Subscribe(lifecycleName, ServiceLifecycleStage.ApplicationServices, Init, Close);
    }

    private Task Init(CancellationToken ct)
    {
        logger.LogDebug("StateHandlerGrainStorage {Name} initialized for ServiceId={ServiceId}", name, clusterOptions.ServiceId);
        return Task.CompletedTask;
    }

    private Task Close(CancellationToken ct) => Task.CompletedTask;

    public Task ReadStateAsync<T>(string grainType, GrainId grainId, IGrainState<T> grainState)
    {
        var handler = handlerFactory.Get<T>();
        return handler.ReadAsync(grainType, grainId, grainState);
    }

    public Task WriteStateAsync<T>(string grainType, GrainId grainId, IGrainState<T> grainState)
    {
        var handler = handlerFactory.Get<T>();
        return handler.WriteAsync(grainType, grainId, grainState);
    }

    public Task ClearStateAsync<T>(string grainType, GrainId grainId, IGrainState<T> grainState)
    {
        var handler = handlerFactory.Get<T>();
        return handler.ClearAsync(grainType, grainId, grainState);
    }
}
