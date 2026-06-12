using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;
public class DbLoggingCommands(
    IDbLoggingConnection connection,
    IOptionsSnapshot<DatabaseOptions> dbOptions,
    ILogger<IDbLoggingCommands> logger
    ) : BaseDbCommands(connection, dbOptions, logger), IDbTransientLoggingCommands
{
}