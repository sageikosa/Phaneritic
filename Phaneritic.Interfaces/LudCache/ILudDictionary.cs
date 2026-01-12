namespace Phaneritic.Interfaces.LudCache;
public interface ILudDictionary<TKey, TLud>
    where TKey : struct, IEquatable<TKey>
    where TLud : class, IEquatable<TLud>, ILudCacheable<TKey>
{
    /// <summary>
    /// Enumerate all keys tracked by this dictionary
    /// </summary>
    IEnumerable<TKey> AllKeys();

    /// <summary>
    /// True if this dictionary is tracking the specified key.
    /// </summary>
    bool HasKey(TKey key);

    /// <summary>
    /// Called internally by refresher to set a new dictionary
    /// </summary>
    void SetValues(IDictionary<TKey, TLud> dictionary);

    /// <summary>
    /// Per process refresh call counter
    /// </summary>
    /// <remarks>
    /// Starts at 0, indicating not loaded.
    /// </remarks>
    long RefreshCycle { get; }

    /// <summary>
    /// Get a single item by key
    /// </summary>
    TLud? Get(TKey? key);

    /// <summary>
    /// Find and return the first item matching the predicate
    /// </summary>
    TLud? Find(Func<TLud, bool> searchFor);

    /// <summary>
    /// Get all items matching any key in the set. 
    /// </summary>
    /// <remarks>
    /// Returns empty list if none found.
    /// </remarks>
    List<TLud> Get(HashSet<TKey> keys);

    /// <summary>
    /// Get all items
    /// </summary>
    List<TLud> All();
}
