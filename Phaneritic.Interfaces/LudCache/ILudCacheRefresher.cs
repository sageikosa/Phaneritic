namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheRefresher
{
    RefresherKey RefresherKey { get; }
    void Refresh();
}
