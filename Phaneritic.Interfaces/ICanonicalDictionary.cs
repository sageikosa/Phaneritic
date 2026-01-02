namespace Phaneritic.Interfaces;

public interface ICanonicalDictionary<TKey, TValue>
    where TKey: struct, IEquatable<TKey>
{
    bool HasKey(TKey key);
    TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
    TValue? TryGetValue(TKey key);
    TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
    bool TryRemove(TKey key);
    void Clear();
}