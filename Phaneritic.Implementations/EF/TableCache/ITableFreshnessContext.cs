using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Implementations.EF.TableCache;

public interface ITableFreshnessContext 
    : IContributeWork
{
    DbSet<TableFreshness> TableFreshnesses { get; }
}

public interface ITableFreshnessContextTransient 
    : ITableFreshnessContext
{
}
