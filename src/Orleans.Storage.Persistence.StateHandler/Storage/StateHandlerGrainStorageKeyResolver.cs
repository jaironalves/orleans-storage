namespace Orleans.Storage.Persistence.StateHandler.Storage;

internal static class StateHandlerGrainStorageKeyResolver
{
    public static StateHandlerGrainStorageKey Resolve(string grainType, GrainReference reference)
    {
        var key = reference.GetPrimaryKeyString();

        return new StateHandlerGrainStorageKey(grainType, key);
    }
}
