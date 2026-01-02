namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheRefreshAll
{
    void RefreshAll(CancellationToken stoppingToken);
}
