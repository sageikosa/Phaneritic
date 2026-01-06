using Microsoft.EntityFrameworkCore.Design;
using Phaneritic.Implementations.EF;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Models.Operational;
public class OperationalContextFactory
    : IDesignTimeDbContextFactory<OperationalContext>
{
    public OperationalContext CreateDbContext(string[] args)
        => new(
            new NoLoggerFactory(),
            CommonKeysDependencies.GetCommonKeysConfigurators()
            .Concat(OperationalKeysDependencies.GetOperationalKeysConfigurators())
            .Concat(OperationalUnicodeKeysDependencies.GetOperationalUnicodeKeysConfigurators())
            .Concat(OperationalIDsDependencies.GetOperationalIDsConfigurators())
            .Concat(OperationalLongIDsDependencies.GetOperationalLongIDsConfigurators()),
            [new ConstantSchemaNamer(args[0])],
            new ConstantSharedDbConnection(args[1]));
}
