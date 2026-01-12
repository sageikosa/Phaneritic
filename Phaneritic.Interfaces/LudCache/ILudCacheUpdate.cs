using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheUpdate<TRefresh> 
    : IContributeWork
    where TRefresh : class, ILudCacheRefresher
{
    /// <summary>
    /// Used when changing underyling data for a cache.
    /// </summary>
    /// <remarks>
    /// Updates database freshness data and fires off in-process cache refresh.
    /// </remarks>
    void SignalUpdate();
}
