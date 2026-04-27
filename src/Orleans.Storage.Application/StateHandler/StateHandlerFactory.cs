using Microsoft.Extensions.DependencyInjection;
using Orleans.Storage.Persistence.StateHandler.Abstractions;

namespace Orleans.Storage.Application.StateHandler;

//public class StateHandlerFactory(IServiceProvider serviceProvider) : IStateHandlerFactory
//{
//    public IStateHandler<TState> Get<TState>() =>
//        serviceProvider.GetRequiredService<IStateHandler<TState>>() ?? throw new InvalidOperationException($"No state handler registered for type {typeof(TState).FullName}");
//}
