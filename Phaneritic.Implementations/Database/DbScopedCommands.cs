using GyroLedger.CodeInterface.Database;
using Microsoft.Extensions.Logging;

namespace GyroLedger.Kernel.Database;

public class DbScopedCommands(
    IDbScopedConnection connection,
    ILogger<IDbScopedCommands> logger
    ) : BaseDbCommands(connection, logger), IDbTransientCommands
{
}
