using GyroLedger.CodeInterface.LudCache;
using GyroLedger.Kernel.Startup;

namespace GyroLedger.Kernel.LudCache;

public class LudCacheKickStart(
    ILudCacheRefreshAll refreshAll
    ) : IKickStart
{
    protected ILudCacheRefreshAll RefreshAll = refreshAll;

    public bool Startup()
    {
        var _source = new CancellationTokenSource();
        RefreshAll.RefreshAll(_source.Token);
        return true;
    }
}
