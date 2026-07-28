namespace Orleans.Storage.Application.StateHandler.TrackedCollections;

public sealed class TrackedDiff<T>
{
    public List<T> Inserts { get; } = [];

    public List<T> Updates { get; } = [];

    public List<T> Deletes { get; } = [];

    public bool HasChanges =>
        Inserts.Count > 0 ||
        Updates.Count > 0 ||
        Deletes.Count > 0;

    public int TotalChanges =>
        Inserts.Count +
        Updates.Count +
        Deletes.Count;
}
