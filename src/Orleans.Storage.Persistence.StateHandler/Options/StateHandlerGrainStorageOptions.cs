using Orleans.Storage.Persistence.StateHandler.Abstractions;
using Orleans.Storage.Persistence.StateHandler.Storage;

namespace Orleans.Storage.Persistence.StateHandler.Options;

public class StateHandlerGrainStorageOptions
{
    internal readonly Dictionary<Type, Type> Handlers = [];

    /// <summary>
    ///<para>Registers a state handler of the specified type for the given state type to be resolved by the dependency injection container.</para> 
    ///<para>The handler must have a constructor that receives <see cref="Storage.StateHandlerContext"/> as a parameter.</para>
    /// </summary>
    /// <typeparam name="TState">The type of state that the handler will manage.</typeparam>
    /// <typeparam name="THandler">The type of the state handler to register.</typeparam>    
    public StateHandlerGrainStorageOptions AddStateHandler<TState, THandler>()
        where THandler : class, IStateHandler<TState>
    {
        Handlers[typeof(TState)] = typeof(THandler);

        ValidateConstructor(typeof(THandler));

        return this;
    }

    private static void ValidateConstructor(Type handlerType)
    {
        var hasCtor = handlerType
            .GetConstructors()
            .Any(ctor =>
            {
                var parameters = ctor.GetParameters();
                return parameters.Any(p => p.ParameterType == typeof(StateHandlerContext));
            });

        if (!hasCtor)
        {
            throw new InvalidOperationException(
                $"{handlerType.Name} must have a constructor that receives {nameof(StateHandlerContext)}");
        }
    }
}
