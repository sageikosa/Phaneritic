using System.Data.Common;
using GyroLedger.CodeInterface.Database;
using Microsoft.Data.SqlClient;

namespace GyroLedger.Kernel.Database;

public abstract class BaseDbConnection(
    string connectionString
    ) : IBaseDbConnection
{
    private readonly DbConnection _Connection = new SqlConnection(connectionString);

    public DbConnection Connection => _Connection;

    public void Dispose()
    {
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
