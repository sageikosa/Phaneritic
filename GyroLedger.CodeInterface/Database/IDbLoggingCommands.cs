using GyroLedger.CodeInterface.CommitWork;
using System.Data.Common;

namespace GyroLedger.CodeInterface.Database;
public interface IDbLoggingCommands : IBaseDbCommands
{
}

public interface IDbTransientLoggingCommands : IDbLoggingCommands
{
}

