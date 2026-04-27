using Microsoft.Extensions.DependencyInjection;
using Orleans.Storage.Persistence.StateHandler.Abstractions;
using Orleans.Storage.Persistence.StateHandler.Options;
using System.Collections.Concurrent;

namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal class StateHandlerFactory(
    IServiceProvider serviceProvider,
    string providerName,
    StateHandlerGrainStorageOptions options) : IStateHandlerFactory
{   
    private readonly ConcurrentDictionary<Type, object> _cache = new();

    public IStateHandler<TState> Get<TState>()
    {
        var handler = GetHandler(typeof(TState));
        return (IStateHandler<TState>)handler;
    }

    private object GetHandler(Type stateType)
    {
        return _cache.GetOrAdd(stateType, t =>
        {
            if (!options.Handlers.TryGetValue(t, out var handlerType))
                throw new InvalidOperationException(
                    $"Handler não registrado para {t.Name}");

            return ActivatorUtilities.CreateInstance(
                serviceProvider,
                handlerType,
                providerName);
        });
    }
}
