using GyroLedger.CodeInterface.CommitWork;
using GyroLedger.CodeInterface.LudCache;
using GyroLedger.Kernel.EF.TableCache;

namespace GyroLedger.Kernel.LudCache;

public class LudCacheUpdate<TRefresh>(
    ITableFreshnessContextTransient context,
    ILudCacheRefreshAll refreshAll,
    IEnumerable<ILudCacheRefresher> allRefreshers
    ) : ILudCacheUpdate<TRefresh>
    where TRefresh : class, ILudCacheRefresher
{
    protected ITableFreshnessContextTransient Context = context;
    protected ILudCacheRefreshAll RefreshAll = refreshAll;
    protected TRefresh? Refresher = allRefreshers.OfType<TRefresh>().FirstOrDefault();

    public void SignalUpdate()
    {
        if (Refresher != null)
        {
            // update shared freshness
            var _fresh = Context.TableFreshnesses.Find(Refresher.RefresherKey);
            if (_fresh == null)
            {
                Context.TableFreshnesses.Add(new TableFreshness
                {
                    TableKey = Refresher.RefresherKey,
                    ConcurrencyCheck = [],
                    LastUpdate = DateTimeOffset.Now
                });
            }
            else
            {
                _fresh.LastUpdate = DateTimeOffset.Now;
            }
        }
    }

    public IEnumerable<IContributeWork> ContributeWork()
    {
        yield return Context;
        yield break;
    }

    public IEnumerable<IContributeWork> ContributeAfterWork()
    {
        RefreshAll.RefreshAll(new CancellationTokenSource().Token);
        yield return Context;
        yield break;
    }
}
