namespace GyroLedger.CodeInterface.LudCache;

public record FreshnessStatus
{
    public RefresherKey TableKey { get; set; }
    public DateTimeOffset FreshFrom { get; set; }
}
