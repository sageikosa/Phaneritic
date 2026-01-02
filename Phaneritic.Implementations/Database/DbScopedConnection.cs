using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public class DbScopedConnection(
    IOptionsSnapshot<GyroDatabaseOptions> options
    ) : BaseDbConnection(options.Value.StandardConnectString), IDbScopedConnection, IDisposable
{
}
