using GyroLedger.CodeInterface.CommitWork;
using GyroLedger.CodeInterface.Database;
using GyroLedger.Kernel.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Transactions;

namespace GyroLedger.Kernel.CommitWork;

public class WorkCommitter(
    IOptionsSnapshot<GyroDatabaseOptions> options,
    IEnumerable<IDbErrorWrap> wrappers,
    ILogger<IWorkCommitter> logger
        ) : IWorkCommitter
{
    protected readonly IList<IDbErrorWrap> Wrappers = [.. wrappers];
    protected readonly ILogger<IWorkCommitter> Logger = logger;

    public void CommitWork(params List<IContributeWork> contributors)
    {
        if (contributors.Count != 0)
        {
            var _ticksPerSecond = Stopwatch.Frequency;
            var _timer = Stopwatch.StartNew();
            try
            {
                Wrappers.DoErrorWrap(() =>
                {
                    // scope marker for transaction "using"
                    {
                        var _contributors = new List<IContributeWork>();
                        using var _scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions
                                {
                                    IsolationLevel = options.Value.IsolationLevel,
                                    Timeout = TransactionManager.DefaultTimeout
                                });
                        foreach (var _contrib in contributors)
                        {
                            // unique commits
                            if (!_contributors.Contains(_contrib))
                            {
                                _contributors.Add(_contrib);

                                // make copies for this sweep
                                var _outbound = _contrib
                                    .ContributeWork()
                                    .Distinct()
                                    .Where(_c => !_contributors.Contains(_c))
                                    .ToList();
                                if (_outbound.Count != 0)
                                {
                                    foreach (var _nc in _outbound)
                                    {
                                        if (Logger.IsEnabled(LogLevel.Information))
                                        {
                                            Logger.LogInformation(@"nested contributor '{name}'", _nc.GetType().FullName);
                                        }
                                    }
                                    _contributors.AddRange(_outbound);
                                }

                                var _time = _timer.Elapsed;
                                if (Logger.IsEnabled(LogLevel.Information))
                                {
                                    Logger.LogInformation(@"work finished for '{name}' (total={total}) at MS={offset}=(1000*{ticks}/{frequency})",
                                        _contrib.GetType().FullName, _contributors.Count, 1000 * (decimal)_time.Ticks / _ticksPerSecond, _time.Ticks, _ticksPerSecond);
                                }
                            }
                        }
                        _scope.Complete();

                        var _allWorkDone = _timer.Elapsed;
                        if (Logger.IsEnabled(LogLevel.Information))
                        {
                            Logger.LogInformation(@"all work done (total={count}) in MS={duration}=(1000*{ticks}/{frequency}) ",
                                _contributors.Count, 1000 * (decimal)_allWorkDone.Ticks / _ticksPerSecond, _allWorkDone.Ticks, _ticksPerSecond);
                        }
                    }

                    // transaction complete above
                    {
                        var _afterWork = new List<IContributeWork>();
                        foreach (var _contrib in contributors)
                        {
                            if (!_afterWork.Contains(_contrib))
                            {
                                _afterWork.Add(_contrib);
                                var outbound = _contrib
                                    .ContributeAfterWork()
                                    .Distinct()
                                    .Where(_c => !_afterWork.Contains(_c))
                                    .ToList();
                                if (outbound.Count != 0)
                                {
                                    foreach (var _y in outbound)
                                    {
                                        if (Logger.IsEnabled(LogLevel.Information))
                                        {
                                            Logger.LogInformation(@"nested after contributor '{name}'", _y.GetType().FullName);
                                        }
                                    }
                                    _afterWork.AddRange(outbound);
                                }

                                var _offset = _timer.Elapsed;
                                if (Logger.IsEnabled(LogLevel.Information))
                                {
                                    Logger.LogInformation(@"after work finished for '{name}' (total={total}) at MS={offset}=(1000*{ticks}/{frequency})",
                                        _contrib.GetType().FullName, _afterWork.Count, 1000 * (decimal)_offset.Ticks / _ticksPerSecond, _offset.Ticks, _ticksPerSecond);
                                }
                            }
                        }

                        var _time = _timer.Elapsed;
                        if (Logger.IsEnabled(LogLevel.Information))
                        {
                            Logger.LogInformation(@"all after work done (total={count}) at MS={duration}=(1000*{ticks}/{frequency}) ",
                                _afterWork.Count, 1000 * (decimal)_time.Ticks / _ticksPerSecond, _time.Ticks, _ticksPerSecond);
                        }
                    }
                });
            }
            finally
            {
                var _span = _timer.Elapsed;
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation(@"CommitWorks={count} in MS={duration}=(1000*{ticks}/{frequency}) ",
                        contributors.Count, 1000 * (decimal)_span.Ticks / _ticksPerSecond, _span.Ticks, _ticksPerSecond);
                }
            }
        }
    }
}
