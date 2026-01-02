using GyroLedger.CodeInterface.Database;
using Microsoft.Extensions.Options;

namespace GyroLedger.Kernel.Database;
public class DbLoggingConnection(
    IOptionsSnapshot<GyroDatabaseOptions> options
    ) : BaseDbConnection(options.Value.LoggingConnectString), IDbLoggingConnection
{
}
