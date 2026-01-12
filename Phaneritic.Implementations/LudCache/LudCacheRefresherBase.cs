using Phaneritic.Interfaces;
using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Implementations.LudCache;
public abstract class LudCacheRefresherBase<TKey, TLud, TModel>(
    IPackRecord<TModel, TLud> packer,
    ILudDictionary<TKey, TLud> dtoDictionary
    ) : ILudCacheRefresher
    where TKey : struct, IEquatable<TKey>
    where TLud : class, IEquatable<TLud>, ILudCacheable<TKey>
    where TModel : class
{
    protected readonly ILudDictionary<TKey, TLud> DtoDictionary = dtoDictionary;

    public abstract RefresherKey RefresherKey { get; }

    public void Refresh()
        => DtoDictionary.SetValues(GetDictionary());

    /// <summary>
    /// Override to query DbContext for models
    /// </summary>
    protected abstract IEnumerable<TModel> GetModels();

    /// <summary>
    /// Override to extract key from dto
    /// </summary>
    protected abstract TKey GetKey(TLud dto);

    /// <summary>
    /// Feed models from GetModels to packer, then convert to dictionary using GetKey.
    /// </summary>
    protected Dictionary<TKey, TLud> GetDictionary()
        => packer.GetDtos(GetModels()).ToDictionary(_d => GetKey(_d));
}
