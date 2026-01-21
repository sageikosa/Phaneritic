using Microsoft.EntityFrameworkCore.Design;
using Phaneritic.Interfaces;
using Phaneritic.Implementations.EF;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Models.Ledgering;
public class LedgeringContextFactory
    : IDesignTimeDbContextFactory<LedgeringContext>
{
    public LedgeringContext CreateDbContext(string[] args)
        => new(
            new NoLoggerFactory(),
            CommonKeysDependencies.GetCommonKeysConfigurators()
            .Concat(LedgeringKeysDependencies.GetLedgeringKeysConfigurators())
            .Concat(LedgeringUnicodeKeysDependencies.GetLedgeringUnicodeKeysConfigurators())
            .Concat(LedgeringIDsDependencies.GetLedgeringIDsConfigurators())
            .Concat(OperationalKeysDependencies.GetOperationalKeysConfigurators())
            .Concat(OperationalUnicodeKeysDependencies.GetOperationalUnicodeKeysConfigurators())
            .Concat(OperationalIDsDependencies.GetOperationalIDsConfigurators())
            .Concat(OperationalLongIDsDependencies.GetOperationalLongIDsConfigurators()),
            [new ConstantSchemaNamer(args[0])],
            new ConstantSharedDbConnection(args[1], args[2], args[3]));
}
