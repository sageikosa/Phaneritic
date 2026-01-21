using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public class DbScopedConnection(
    IOptionsSnapshot<DatabaseOptions> options
    ) : BaseDbConnection(options.Value.StandardConnectString, options.Value.CaseInsensitiveCollation, options.Value.CaseSensitiveCollation), IDbScopedConnection, IDisposable
{
}
