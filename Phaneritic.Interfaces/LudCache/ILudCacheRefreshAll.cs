namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheRefreshAll
{
    /// <summary>
    /// Runs through all registered refreshers and calls Refresh if needed (unitialized or server freshness date is newer)
    /// </summary>
    void RefreshAll(CancellationToken stoppingToken);
}
