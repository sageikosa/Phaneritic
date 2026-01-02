namespace GyroLedger.CodeInterface.LudCache;

public interface ILudCacheRefreshAll
{
    void RefreshAll(CancellationToken stoppingToken);
}
