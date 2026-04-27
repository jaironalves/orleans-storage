namespace Orleans.Storage.Persistence.StateHandler.Abstractions;

internal interface IStateHandlerFactory
{
    IStateHandler<TState> Get<TState>();    
}
