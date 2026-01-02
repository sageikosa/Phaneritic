using GyroLedger.CodeInterface.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace GyroLedger.Kernel.Database;

/// <summary>
/// Handles errors with delays Performs the delay.
/// </summary>
public class SqlErrorRetry(
    IOptionsSnapshot<GyroDatabaseOptions> options,
    ILogger<SqlErrorRetry> logger
        ) : IDbErrorWrap
{
    private readonly HashSet<int> _ErrNums = [3960, 1205];

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
                    action?.Invoke();
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
