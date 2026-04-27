using Orleans.Storage.Persistence.StateHandler.Abstractions;

namespace Orleans.Storage.Persistence.StateHandler.Options;

public class StateHandlerGrainStorageOptions
{
    internal readonly Dictionary<Type, Type> Handlers = [];

    public StateHandlerGrainStorageOptions AddStateHandler<TState, THandler>()
        where THandler : class, IStateHandler<TState>
    {
        Handlers[typeof(TState)] = typeof(THandler);
        return this;
    }
}
