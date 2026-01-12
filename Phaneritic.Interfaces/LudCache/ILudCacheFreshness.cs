namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheFreshness
{
    /// <summary>
    /// Might be useful for a monitoring display or debugging
    /// </summary>
    List<FreshnessStatus> GetAllFreshnesses();

    /// <summary>
    /// Get freshness for a specific refresher
    /// </summary>
    DateTimeOffset? GetFreshness(RefresherKey tableKey);

    /// <summary>
    /// Used by ILudCacheRefreshAll to check if a refresh is needed
    /// </summary>
    bool IsRefreshNeeded(RefresherKey tableKey, DateTimeOffset latestDate);

    /// <summary>
    /// Used by ILudCacheRefreshAll to set new freshness date
    /// </summary>
    bool SetFreshness(RefresherKey tableKey, DateTimeOffset newDate);
}
