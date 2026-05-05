using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Phaneritic.Implementations.Database;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Database;
using System.Diagnostics;
using System.Transactions;

namespace Phaneritic.Implementations.CommitWork;

public class WorkCommitter(
    IOptionsSnapshot<DatabaseOptions> options,
    IEnumerable<IDbErrorWrap> wrappers,
    ILogger<IWorkCommitter> logger
        ) : IWorkCommitter
{
    protected readonly IList<IDbErrorWrap> Wrappers = [.. wrappers];
    protected readonly ILogger<IWorkCommitter> Logger = logger;

    public async Task CommitWork(CancellationToken cancellationToken, params List<IContributeWork> contributors)
    {
        if (contributors.Count != 0)
        {
            var _ticksPerSecond = Stopwatch.Frequency;
            var _timer = Stopwatch.StartNew();
            try
            {
                // go through all wrappers using extension method on list
                await Wrappers.DoErrorWrap(async () =>
                {
                    // scope marker for transaction "using"
                    {
                        using var _scope = new TransactionScope(TransactionScopeOption.Required,
                                new TransactionOptions
                                {
                                    IsolationLevel = options.Value.IsolationLevel,
                                    Timeout = TransactionManager.DefaultTimeout
                                });

                        // duplicate block list
                        var _track = new List<IContributeWork>();

                        // processing
                        var _contributors = new Queue<IContributeWork>(contributors);
                        while (_contributors.TryDequeue(out var _contrib))
                        {
                            // block duplication commits
                            if (!_track.Contains(_contrib))
                            {
                                // by tracking, we block multiple attempts to get in here
                                _track.Add(_contrib);

                                // contribute work and get all things that spun out of it
                                var _outbound = await _contrib
                                    .ContributeWork(cancellationToken)
                                    .Distinct()
                                    .Where(_c => !_contributors.Contains(_c))
                                    .ToListAsync(cancellationToken);
                                if (_outbound.Count != 0)
                                {
                                    // enqueue each thing that spun out
                                    foreach (var _nc in _outbound)
                                    {
                                        if (Logger.IsEnabled(LogLevel.Information))
                                        {
                                            Logger.LogInformation(@"nested contributor '{name}'", _nc.GetType().FullName);
                                        }

                                        // duplicate checks after dequeue will block updates from a contributor more than once
                                        _contributors.Enqueue(_nc);
                                    }
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
                        // duplicate block list
                        var _track = new List<IContributeWork>();

                        var _afterWork = new Queue<IContributeWork>(contributors);
                        while (_afterWork.TryDequeue(out var _contrib))
                        {
                            // unique after work calls
                            if (!_track.Contains(_contrib))
                            {
                                // by tracking, we block multiple attempts to get in here
                                _track.Add(_contrib);

                                // after work contribs
                                var outbound = await _contrib
                                    .ContributeAfterWork(cancellationToken)
                                    .Distinct()
                                    .Where(_c => !_afterWork.Contains(_c))
                                    .ToListAsync(cancellationToken);
                                if (outbound.Count != 0)
                                {
                                    // enqueue each thing that spun out
                                    foreach (var _y in outbound)
                                    {
                                        if (Logger.IsEnabled(LogLevel.Information))
                                        {
                                            Logger.LogInformation(@"nested after contributor '{name}'", _y.GetType().FullName);
                                        }
                                        _afterWork.Enqueue(_y);
                                    }
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
                }, cancellationToken);
            }
            finally
            {
                var _span = _timer.Elapsed;
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation(@"ContributeWorks={count} in MS={duration}=(1000*{ticks}/{frequency}) ",
                        contributors.Count, 1000 * (decimal)_span.Ticks / _ticksPerSecond, _span.Ticks, _ticksPerSecond);
                }
            }
        }
    }
}
