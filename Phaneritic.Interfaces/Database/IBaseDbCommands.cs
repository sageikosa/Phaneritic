using Phaneritic.Interfaces.CommitWork;
using System.Data.Common;

namespace Phaneritic.Interfaces.Database;
public interface IBaseDbCommands : IContributeWork, IDisposable
{
    DbCommand AddDbCommand(string text);
    void ClearDbCommands();
}

