using GyroLedger.CodeInterface.CommitWork;

namespace GyroLedger.CodeInterface.LudCache;

public interface ILudCacheFreshnessNotify
{
    IEnumerable<RefresherKey> TableKeys { get; }
    IEnumerable<IContributeWork> Notify();
}
