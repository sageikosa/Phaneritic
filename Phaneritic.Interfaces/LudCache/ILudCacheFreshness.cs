namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheFreshness
{
    List<FreshnessStatus> GetAllFreshnesses();
    DateTimeOffset? GetFreshness(RefresherKey tableKey);
    bool IsRefreshNeeded(RefresherKey tableKey, DateTimeOffset latestDate);
    bool SetFreshness(RefresherKey tableKey, DateTimeOffset newDate);
}
