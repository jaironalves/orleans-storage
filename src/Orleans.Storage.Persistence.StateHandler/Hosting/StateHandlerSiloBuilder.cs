using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Storage.Persistence.StateHandler.Abstractions;
using Orleans.Storage.Persistence.StateHandler.Options;
using Orleans.Storage.Persistence.StateHandler.Storage;

namespace Orleans.Storage.Persistence.StateHandler.Hosting;

public class StateHandlerSiloBuilder(ISiloBuilder siloBuilder, string name) :
    ISiloBuilder
{
    public IServiceCollection Services => siloBuilder.Services;
    
    public IConfiguration Configuration => siloBuilder.Configuration;

    /// <summary>
    ///<para>Registers a state handler of the specified type for the given state type to be resolved by the dependency injection container.</para> 
    ///<para>The handler must have a constructor that receives <see cref="StateHandlerContext"/> as a parameter.</para>
    /// </summary>
    /// <typeparam name="TState">The type of state that the handler will manage.</typeparam>
    /// <typeparam name="THandler">The type of the state handler to register.</typeparam>    
    public StateHandlerSiloBuilder AddStateHandler<TState, THandler>()
        where THandler : class, IStateHandler<TState>
    {
        var handlerType = typeof(THandler);

        ValidateConstructor(handlerType);

        Services.Configure<StateHandlerGrainStorageOptions>(name, options =>
        {
            options.Handlers[typeof(TState)] = handlerType;
        });

        Services.TryAddScoped<IStateHandler<TState>, THandler>();

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
