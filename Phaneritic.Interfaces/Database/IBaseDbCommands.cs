using Phaneritic.Interfaces.CommitWork;
using System.Data.Common;

namespace Phaneritic.Interfaces.Database;
public interface IBaseDbCommands : IContributeWork, IDisposable
{
    /// <summary>
    /// Add a command.
    /// </summary>
    /// <remarks>if priority is not supplied, the index of the cammand in the list at time of add will be used for execution order</remarks>
    /// <param name="text"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    DbCommand AddDbCommand(string text, int? priority = null);

    void ClearDbCommands();
}

