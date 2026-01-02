using Microsoft.Extensions.Logging;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;
public class DbLoggingCommands(
    IDbLoggingConnection connection,
    ILogger<IDbLoggingCommands> logger
    ) : BaseDbCommands(connection, logger), IDbTransientLoggingCommands
{
}