using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Implementations.LudCache;

/// <summary>
/// Provides a base class for cache freshness notification handlers that operate on a specific type of cache refresher.
/// </summary>
/// <typeparam name="TRefresh">The type of cache refresher to be used for notifications. Must implement <see cref="ILudCacheRefresher"/>.</typeparam>
/// <param name="allRefreshers">A collection of all available cache refreshers. Used to select the refresher of type 
/// <typeparamref name="TRefresh"/>.</param>
public abstract class LudCacheFreshnessNotify<TRefresh>(
    IEnumerable<ILudCacheRefresher> allRefreshers
    ) : ILudCacheFreshnessNotify
    where TRefresh : class, ILudCacheRefresher
{
    protected readonly TRefresh? Refresher = allRefreshers.OfType<TRefresh>().FirstOrDefault();

    public IEnumerable<RefresherKey> TableKeys => [Refresher?.RefresherKey ?? default];

    /// <summary>Override to handle notification</summary>
    public abstract IEnumerable<IContributeWork> Notify();
}
