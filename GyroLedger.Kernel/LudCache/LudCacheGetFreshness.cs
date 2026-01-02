using GyroLedger.CodeInterface.LudCache;

namespace GyroLedger.Kernel.LudCache;

public class LudCacheGetFreshness<TRefresh>(
    IEnumerable<ILudCacheRefresher> allRefreshers,
    ILudCacheFreshness cacheFreshness
    ) : ILudCacheGetFreshness<TRefresh>
    where TRefresh : class, ILudCacheRefresher
{
    protected TRefresh? Refresher = allRefreshers.OfType<TRefresh>().FirstOrDefault();
    protected ILudCacheFreshness CacheFreshness = cacheFreshness;

    public DateTimeOffset? GetFreshness()
        => CacheFreshness.GetFreshness(Refresher?.RefresherKey ?? default);
}
