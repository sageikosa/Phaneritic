using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Database;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Phaneritic.Implementations.Database;

public abstract class BaseDbCommands(
    IBaseDbConnection connection,
    ILogger<IBaseDbCommands> logger
        ) : IBaseDbCommands
{
    protected readonly List<(SqlCommand cmd, int priority)> SqlCommands = [];
    protected readonly IBaseDbConnection Connection = connection;
    protected readonly ILogger<IBaseDbCommands> Logger = logger;

    public DbCommand AddDbCommand(string sqlText, int? priority = null)
    {
        var _cmd = new SqlCommand(sqlText, Connection.Connection as SqlConnection);

        // use natural add-order priority unless explicitly provided
        int _priority = priority ?? SqlCommands.Count;
        SqlCommands.Add((_cmd, _priority));
        return _cmd;
    }

    public void ClearDbCommands()
    {
        foreach (var (_cmd, _) in SqlCommands)
        {
            _cmd.Dispose();
        }
        SqlCommands.Clear();
    }

    public void Dispose()
    {
        ClearDbCommands();
        GC.SuppressFinalize(this);
    }

    public IEnumerable<IContributeWork> ContributeAfterWork()
    {
        ClearDbCommands();

        // no extras
        yield break;
    }

    public IEnumerable<IContributeWork> ContributeWork()
    {
        // assuming no need to close
        var _mustClose = false;
        if (Connection.Connection.State != System.Data.ConnectionState.Open)
        {
            // unless it had to be opened
            _mustClose = true;
            Connection.Connection.Open();
        }

        // run each command in priority order
        var _frequency = Stopwatch.Frequency;
        var _stopWatch = Stopwatch.StartNew();
        foreach (var (_cmd, _priority) in SqlCommands.OrderBy(_c => _c.priority))
        {
            _stopWatch.Restart();
            var _count = _cmd.ExecuteNonQuery();
            var _span = _stopWatch.Elapsed;
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation(@"Records={count} in MS={duration}=(1000*{ticks}/{frequency}) for SQL: ""{sql}""",
                    _count, 1000 * (decimal)_span.Ticks / _frequency, _span.Ticks, _frequency, _cmd.CommandText);
            }
        }
        _stopWatch.Stop();

        if (_mustClose)
        {
            // since this code opened the connection, it should close it also...
            Connection.Connection.Close();
        }

        // no extras
        yield break;
    }
}
