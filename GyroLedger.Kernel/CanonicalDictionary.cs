using GyroLedger.CodeInterface;
using System.Collections.Concurrent;

namespace GyroLedger.Kernel;

public class CanonicalDictionary<TKey, TValue>
    : ICanonicalDictionary<TKey, TValue>
    where TKey : struct, IEquatable<TKey>
{
    private readonly ConcurrentDictionary<TKey, TValue> _Dictionary = new();

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> addValueFactory)
        => _Dictionary.GetOrAdd(key, addValueFactory);

    public TValue? TryGetValue(TKey key)
        => _Dictionary.TryGetValue(key, out TValue? _value) ? _value : default;

    public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        => _Dictionary.AddOrUpdate(key, addValue, updateValueFactory);

    public bool TryRemove(TKey key)
        => _Dictionary.TryRemove(key, out _);

    public void Clear()
        => _Dictionary.Clear();

    public bool HasKey(TKey key)
        => _Dictionary.ContainsKey(key);
}
