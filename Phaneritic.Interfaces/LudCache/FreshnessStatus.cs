namespace Phaneritic.Interfaces.LudCache;

public record FreshnessStatus
{
    public RefresherKey TableKey { get; set; }
    public DateTimeOffset FreshFrom { get; set; }
}
