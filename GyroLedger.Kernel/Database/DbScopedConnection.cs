using GyroLedger.CodeInterface.Database;
using Microsoft.Extensions.Options;

namespace GyroLedger.Kernel.Database;

public class DbScopedConnection(
    IOptionsSnapshot<GyroDatabaseOptions> options
    ) : BaseDbConnection(options.Value.StandardConnectString), IDbScopedConnection, IDisposable
{
}
