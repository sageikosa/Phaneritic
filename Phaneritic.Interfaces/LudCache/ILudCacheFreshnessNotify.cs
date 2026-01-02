using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheFreshnessNotify
{
    IEnumerable<RefresherKey> TableKeys { get; }
    IEnumerable<IContributeWork> Notify();
}
