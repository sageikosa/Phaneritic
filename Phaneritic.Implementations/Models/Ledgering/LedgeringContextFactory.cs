using Microsoft.EntityFrameworkCore.Design;
using Phaneritic.Interfaces;
using Phaneritic.Implementations.EF;
using Phaneritic.Interfaces.Ledgering;

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
            .Concat(LedgeringIDsDependencies.GetLedgeringIDsConfigurators()),
            [new ConstantSchemaNamer(args[0])],
            new ConstantSharedDbConnection(args[1]));
}
