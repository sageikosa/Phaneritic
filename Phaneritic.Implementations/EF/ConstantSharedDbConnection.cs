using GyroLedger.CodeInterface.Database;
using Microsoft.Data.SqlClient;

namespace GyroLedger.Kernel.EF;

public class ConstantSharedDbConnection(string connection) 
    : IBaseDbConnection, IDbScopedConnection, IDbLoggingConnection
{
    // @"Server=.;Database=GLWALK;User ID=GLWALK;Password=GLWALK;TrustServerCertificate=true;"
    public System.Data.Common.DbConnection Connection
        => new SqlConnection(connection);

    public void Dispose()
    {
        Connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
