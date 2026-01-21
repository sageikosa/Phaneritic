using Microsoft.Data.SqlClient;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.EF;

public class ConstantSharedDbConnection(
    string connection,
    string caseInsensitiveCollation,
    string caseSensitiveCollation
    ) : IBaseDbConnection, IDbScopedConnection, IDbLoggingConnection
{
    // @"Server=.;Database=GLWALK;User ID=GLWALK;Password=GLWALK;TrustServerCertificate=true;"
    public System.Data.Common.DbConnection Connection
        => new SqlConnection(connection);

    public string CaseInsensitiveCollation => caseInsensitiveCollation;

    public string CaseSensitiveCollation => caseSensitiveCollation;

    public void Dispose()
    {
        Connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
