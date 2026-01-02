using Phaneritic.Interfaces;
using Phaneritic.Interfaces.LudCache;
using System.Collections.Concurrent;

namespace Phaneritic.Implementations.LudCache;

public class LudCacheFreshness 
    : ILudCacheFreshness
{
    private readonly ConcurrentDictionary<RefresherKey, DateTimeOffset> _Freshness = new();

    public bool IsRefreshNeeded(RefresherKey tableKey, DateTimeOffset latestDate) 
        => !_Freshness.TryGetValue(tableKey, out var _cacheDate) || latestDate > _cacheDate;

    public bool SetFreshness(RefresherKey tableKey, DateTimeOffset newDate) 
        => _Freshness.AddOrUpdate(tableKey, newDate, (key, exist) => exist > newDate ? exist : newDate) == newDate;

    public DateTimeOffset? GetFreshness(RefresherKey tableKey)
        => _Freshness.TryGetValue(tableKey, out var _freshness) ? _freshness : null;

    public List<FreshnessStatus> GetAllFreshnesses()
        => [.. _Freshness.Select(_kvp => new FreshnessStatus
        {
            TableKey = _kvp.Key,
            FreshFrom = _kvp.Value
        })];

}
