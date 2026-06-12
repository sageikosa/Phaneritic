using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public class DbScopedCommands(
    IDbScopedConnection connection,
    IOptionsSnapshot<DatabaseOptions> dbOptions,
    ILogger<IDbScopedCommands> logger
    ) : BaseDbCommands(connection, dbOptions, logger), IDbTransientCommands
{
}
