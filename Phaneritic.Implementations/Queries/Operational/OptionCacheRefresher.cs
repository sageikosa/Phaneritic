using Phaneritic.Implementations.LudCache;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Queries.Operational;
public class OptionCacheRefresher(
    ICanonicalDictionary<(ProcessNodeKey ProcessNodeKey, OptionTypeKey OptionTypeKey), OptionValue?> optionCache,
    IEnumerable<ILudCacheRefresher> allRefreshers
    ) : LudCacheFreshnessNotify<ProcessNodeKeyRefresh>(allRefreshers), IContributeWork
{
    public IEnumerable<IContributeWork> ContributeAfterWork()
    {
        optionCache.Clear();
        return [];
    }

    public IEnumerable<IContributeWork> ContributeWork()
        => [];

    public override IEnumerable<IContributeWork> Notify()
        => [this];
}
