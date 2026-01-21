using System.Data.Common;
using Microsoft.Data.SqlClient;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public abstract class BaseDbConnection(
    string connectionString,
    string caseInsensitiveCollation,
    string caseSensitiveCollation
    ) : IBaseDbConnection
{
    private readonly DbConnection _Connection = new SqlConnection(connectionString);

    public DbConnection Connection => _Connection;

    public string CaseInsensitiveCollation => caseInsensitiveCollation;

    public string CaseSensitiveCollation => caseSensitiveCollation;

    public void Dispose()
    {
        Connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
