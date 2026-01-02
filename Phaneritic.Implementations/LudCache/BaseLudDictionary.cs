using GyroLedger.CodeInterface.LudCache;
using System.Collections.Frozen;

namespace GyroLedger.Kernel.LudCache;
public class BaseLudDictionary<TKey, TLud>(
    ) : ILudDictionary<TKey, TLud>
    where TKey : struct, IEquatable<TKey>
    where TLud : class, IEquatable<TLud>, ILudCacheable<TKey>
{
    private long _Generation = 0;
    private FrozenDictionary<TKey, TLud> _Cache = new Dictionary<TKey, TLud>().ToFrozenDictionary();

    public IEnumerable<TKey> AllKeys()
    {
        var _c = _Cache;
        return [.. _c.Keys.Select(_k => _k)];
    }

    public bool HasKey(TKey key)
        => _Cache.ContainsKey(key);

    public void SetValues(IDictionary<TKey, TLud> dictionary)
    {
        _Generation++;
        _Cache = dictionary.ToFrozenDictionary();
    }

    public long RefreshCycle => _Generation;

    public TLud? Find(Func<TLud, bool> searchFor)
    {
        var _c = _Cache;
        return _c.Values.FirstOrDefault(searchFor);
    }

    public TLud? Get(TKey? key)
        => _Cache.TryGetValue(key ?? default, out var _return) ? _return : null;

    public List<TLud> Get(HashSet<TKey> keys)
    {
        var _c = _Cache;
        return [..keys
            .Select(_k => _c.TryGetValue(_k, out var _l) ? _l : null)
            .OfType<TLud>()];
    }

    public List<TLud> All()
    {
        var _c = _Cache;
        return [.. _c.Values.Select(_l => _l)];
    }
}
