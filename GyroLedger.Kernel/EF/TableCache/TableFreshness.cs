using GyroLedger.CodeInterface;
namespace GyroLedger.Kernel.EF.TableCache;

public class TableFreshness
{
    public required RefresherKey TableKey { get; set; }
    public DateTimeOffset LastUpdate { get; set; }
    public required byte[] ConcurrencyCheck { get; set; }
}
