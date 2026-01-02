using GyroLedger.CodeInterface.CommitWork;
using Microsoft.EntityFrameworkCore;

namespace GyroLedger.Kernel.EF.TableCache;

public interface ITableFreshnessContext 
    : IContributeWork
{
    DbSet<TableFreshness> TableFreshnesses { get; }
}

public interface ITableFreshnessContextTransient 
    : ITableFreshnessContext
{
}
