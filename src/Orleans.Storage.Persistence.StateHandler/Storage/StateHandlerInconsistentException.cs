using System;

namespace Orleans.Storage.Persistence.StateHandler.Storage
{
    public class StateHandlerInconsistentException : Exception
    {
        public StateHandlerInconsistentException()
        {
        }

        public StateHandlerInconsistentException(string message)
            : base(message)
        {
        }

        public StateHandlerInconsistentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
