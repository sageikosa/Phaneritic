using System.Data.Common;

namespace GyroLedger.CodeInterface.Database;
public interface IBaseDbConnection : IDisposable
{
    DbConnection Connection { get; }
}
