namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheRefresher
{
    /// <summary>
    /// RefreshKey identifying this refresher.
    /// </summary>
    RefresherKey RefresherKey { get; }

    /// <summary>
    /// Refresh the dictionary.
    /// </summary>
    void Refresh();
}
