using GyroLedger.CodeInterface.CommitWork;
using System.Data.Common;

namespace GyroLedger.CodeInterface.Database;
public interface IBaseDbCommands : IContributeWork, IDisposable
{
    DbCommand AddDbCommand(string text);
    void ClearDbCommands();
}

