using GyroLedger.CodeInterface.CommitWork;

namespace GyroLedger.CodeInterface.LudCache;

public interface ILudCacheUpdate<TRefresh> 
    : IContributeWork
    where TRefresh : class, ILudCacheRefresher
{
    void SignalUpdate();
}
