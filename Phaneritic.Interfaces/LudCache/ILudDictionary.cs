namespace Phaneritic.Interfaces.LudCache;
public interface ILudDictionary<TKey, TLud>
    where TKey : struct, IEquatable<TKey>
    where TLud : class, IEquatable<TLud>, ILudCacheable<TKey>
{
    IEnumerable<TKey> AllKeys();
    bool HasKey(TKey key);

    void SetValues(IDictionary<TKey, TLud> dictionary);
    long RefreshCycle { get; }

    TLud? Get(TKey? key);
    TLud? Find(Func<TLud, bool> searchFor);
    List<TLud> Get(HashSet<TKey> keys);
    List<TLud> All();
}
