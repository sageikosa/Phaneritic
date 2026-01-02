using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Interfaces.LudCache;

public interface ILudCacheUpdate<TRefresh> 
    : IContributeWork
    where TRefresh : class, ILudCacheRefresher
{
    void SignalUpdate();
}
