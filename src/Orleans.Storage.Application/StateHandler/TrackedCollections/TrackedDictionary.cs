using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Orleans.Storage.Application.StateHandler.TrackedCollections;

[GenerateSerializer]
[Alias(nameof(TrackedDictionary<,>))]
public class TrackedDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>
    where TKey : notnull
    where TValue : ITrackedValue<TValue>
{   

    [JsonIgnore]
    [IgnoreDataMember]
    [Id(0)]
    private Dictionary<TKey, TValue> _original = [];
    
    public void Snapshot()
    {
        _original = this.ToDictionary(
            x => x.Key,
            x => x.Value.DeepClone());
    }

    public TrackedDiff<TValue> Diff(
        Func<TValue, TValue, bool> changed)
    {
        var diff = new TrackedDiff<TValue>();

        foreach (var (key, current) in this)
        {
            if (!_original.TryGetValue(key, out var original))
            {
                diff.Inserts.Add(current);
                continue;
            }

            if (changed(original, current))
            {
                diff.Updates.Add(current);
            }
        }

        foreach (var (key, original) in _original)
        {
            if (!ContainsKey(key))
            {
                diff.Deletes.Add(original);
            }
        }

        return diff;
    }
}