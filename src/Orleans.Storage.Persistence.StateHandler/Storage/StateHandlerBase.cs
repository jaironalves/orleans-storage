using Orleans.Storage.Persistence.StateHandler.Abstractions;

namespace Orleans.Storage.Persistence.StateHandler.Storage;

public abstract class StateHandlerBase<TState>(StateHandlerContext context) : IStateHandler<TState>
{
    public StateHandlerContext Context => context;

    public abstract Task ReadAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
    public abstract Task WriteAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
    public abstract Task ClearAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
}
