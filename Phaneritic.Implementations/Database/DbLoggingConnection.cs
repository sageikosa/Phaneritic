using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;
public class DbLoggingConnection(
    IOptionsSnapshot<GyroDatabaseOptions> options
    ) : BaseDbConnection(options.Value.LoggingConnectString), IDbLoggingConnection
{
}
