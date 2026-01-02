using System.Data.Common;
using Microsoft.Data.SqlClient;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

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
