using Microsoft.Extensions.Logging;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public class DbScopedCommands(
    IDbScopedConnection connection,
    ILogger<IDbScopedCommands> logger
    ) : BaseDbCommands(connection, logger), IDbTransientCommands
{
}
