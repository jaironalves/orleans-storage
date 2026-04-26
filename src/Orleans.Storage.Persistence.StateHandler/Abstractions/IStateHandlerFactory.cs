namespace Orleans.Storage.Persistence.StateHandler.Abstractions;

public interface IStateHandlerFactory
{
    IStateHandler<TState> Get<TState>();    
}
