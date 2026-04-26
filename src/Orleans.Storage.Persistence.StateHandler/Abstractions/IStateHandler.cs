namespace Orleans.Storage.Persistence.StateHandler.Abstractions;

public interface IStateHandler<TState>
{
    Task ReadAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
    Task WriteAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
    Task ClearAsync(string grainType, GrainId grainId, IGrainState<TState> grainState);
}
