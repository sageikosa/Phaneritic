using GyroLedger.CodeInterface.Database;
using Microsoft.Extensions.Logging;

namespace GyroLedger.Kernel.Database;
public class DbLoggingCommands(
    IDbLoggingConnection connection,
    ILogger<IDbLoggingCommands> logger
    ) : BaseDbCommands(connection, logger), IDbTransientLoggingCommands
{
}