namespace Phaneritic.Interfaces;

public interface ICanonicalDictionary<TKey, TValue>
    where TKey: struct, IEquatable<TKey>
{
    bool HasKey(in TKey key);
    TValue GetOrAdd(in TKey key, Func<TKey, TValue> valueFactory);
    TValue? TryGetValue(in TKey key);
    TValue AddOrUpdate(in TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
    bool TryRemove(in TKey key);
    void Clear();
}