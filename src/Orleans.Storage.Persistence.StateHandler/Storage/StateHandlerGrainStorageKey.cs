namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal record StateHandlerGrainStorageKey(
    string GrainType,
    string Key
);