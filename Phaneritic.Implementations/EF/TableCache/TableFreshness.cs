using Phaneritic.Interfaces;
namespace Phaneritic.Implementations.EF.TableCache;

public class TableFreshness
{
    public required RefresherKey TableKey { get; set; }
    public DateTimeOffset LastUpdate { get; set; }
    public required byte[] ConcurrencyCheck { get; set; }
}
