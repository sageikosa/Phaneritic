using Phaneritic.Implementations.Startup;
using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Implementations.LudCache;

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
