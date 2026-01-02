using GyroLedger.CodeInterface.CommitWork;
using System.Data.Common;

namespace GyroLedger.CodeInterface.Database;

public interface IDbScopedCommands : IBaseDbCommands
{
}

public interface IDbTransientCommands : IDbScopedCommands
{
}
