namespace GyroLedger.CodeInterface.LudCache;

public interface ILudCacheRefresher
{
    RefresherKey RefresherKey { get; }
    void Refresh();
}
