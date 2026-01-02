using System.Data.Common;

namespace Phaneritic.Interfaces.Database;
public interface IBaseDbConnection : IDisposable
{
    DbConnection Connection { get; }
}
