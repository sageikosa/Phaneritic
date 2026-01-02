using Microsoft.EntityFrameworkCore.Design;
using Phaneritic.Interfaces;

namespace Phaneritic.Implementations.EF.TableCache;

public class TableFreshnessContextFactory 
    : IDesignTimeDbContextFactory<TableFreshnessContext>
{
    public TableFreshnessContext CreateDbContext(string[] args)
    {
        return new TableFreshnessContext(
            new NoLoggerFactory(),
            KernelKeysDependencies.GetKernelKeysConfigurators(),
            [new ConstantSchemaNamer(args[0])],
            new ConstantSharedDbConnection(args[1]));
    }
}
