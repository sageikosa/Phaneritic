using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Phaneritic.Interfaces.Database;
using System.Diagnostics;

namespace Phaneritic.Implementations.Database;

/// <summary>
/// Handles errors with delays Performs the delay.
/// </summary>
public class SqlErrorRetry(
    IOptionsSnapshot<DatabaseOptions> options,
    ILogger<SqlErrorRetry> logger
        ) : IDbErrorWrap
{
    /// <summary>
    /// <para>3960: Snapshot isolation transaction aborted due to update conflict</para>
    /// <para>1205: Transaction deadlocked</para>
    /// <para>2627: Violation of constraint (most likely a PK insertion from a MERGE?)</para>
    /// </summary>
    private readonly HashSet<int> _ErrNums = [3960, 1205, 2627];

    public void ErrorWrap(Action action)
    {
        var _max = options.Value.DbRetryMax;
        var _delay = options.Value.DbRetryDelayMilliseconds;
        var _tries = 0;
        var _processed = false;
        var _ticksPerSecond = Stopwatch.Frequency;
        var _timer = Stopwatch.StartNew();
        try
        {
            while (!_processed)
            {
                try
                {
                    action.Invoke();
                    _processed = true;
                }
                catch (SqlException _sqlEx)
                    when (_ErrNums.Contains(_sqlEx.ErrorCode) && (_tries < _max))
                {
                    _tries++;
                    var _span = _timer.Elapsed;
                    logger.LogWarning(@"error count={tries} at milliseconds={duration}=(1000*{ticks}/{frequency})", _tries,
                        1000 * (decimal)_span.Ticks / _ticksPerSecond, _span.Ticks, _ticksPerSecond);
                    Task.Delay(_delay).Wait();
                }
                catch (SqlException _sqlEx)
                    when (_ErrNums.Contains(_sqlEx.ErrorCode))
                {
                    throw new DbConflictException(_sqlEx.ErrorCode, @"save conflicts on potentially replayable scope action", _sqlEx);
                }
                catch (SqlException _sqlEx)
                {
                    logger.LogError(@"SQL Exception: code={code}", _sqlEx.ErrorCode);
                    throw;
                }
                catch (Exception _exception)
                {
                    logger.LogError(@"exception={exception}", _exception.Message);
                    throw;
                }
            }
        }
        finally
        {
            var _span = _timer.Elapsed;
            if (!_processed)
            {
                logger.LogError(@"{tries} retries exceeded", _tries);
            }
        }
    }
}
