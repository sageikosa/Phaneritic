namespace GyroLedger.CodeInterface.LudCache;

public interface ILudCacheGetFreshness<TRefresh>
    where TRefresh : class, ILudCacheRefresher
{
    DateTimeOffset? GetFreshness();
}
