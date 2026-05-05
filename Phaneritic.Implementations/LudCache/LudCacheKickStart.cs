using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Startup;

namespace Phaneritic.Implementations.LudCache;

public class LudCacheKickStart(
    ILudCacheRefreshAll refreshAll
    ) : IKickStart
{
    protected ILudCacheRefreshAll RefreshAll = refreshAll;

    public bool Startup()
    {
        RefreshAll.RefreshAll();
        return true;
    }
}
