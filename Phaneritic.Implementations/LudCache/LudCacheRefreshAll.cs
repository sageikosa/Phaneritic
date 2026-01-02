using Microsoft.Extensions.Logging;
using Phaneritic.Implementations.EF.TableCache;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Implementations.LudCache;

public class LudCacheRefreshAll(
    IEnumerable<ILudCacheRefresher> refreshers,
    ILogger<ILudCacheRefreshAll> logger,
    IEnumerable<ILudCacheFreshnessNotify> notifiers,
    ITableFreshnessContext tableFreshnessContext,
    ILudCacheFreshness ludCacheFreshness,
    IWorkCommitter workCommitter
    ) : ILudCacheRefreshAll
{
    private readonly IList<ILudCacheRefresher> Refreshers = [.. refreshers];
    private readonly ILogger<ILudCacheRefreshAll> Logger = logger;

    private readonly Dictionary<RefresherKey, List<ILudCacheFreshnessNotify>> Notifiers
        = (from _n in notifiers
           from _k in _n.TableKeys.Distinct()
           group _n by _k)
        .Where(_g => _g != default)
        .ToDictionary(_g => _g.Key, _g => _g.ToList());

    public void RefreshAll(CancellationToken stoppingToken)
    {
        var _actualWork = false;
        var _fromDatabase = tableFreshnessContext.TableFreshnesses
            .Select(_f => _f)
            .ToDictionary(_f => _f.TableKey);
        var _gatherNotifiers = new List<ILudCacheFreshnessNotify>();
        foreach (var _cached in Refreshers)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                if (_fromDatabase.TryGetValue(_cached.RefresherKey, out var _db))
                {
                    if (ludCacheFreshness.IsRefreshNeeded(_cached.RefresherKey, _db.LastUpdate))
                    {
                        if (ludCacheFreshness.SetFreshness(_cached.RefresherKey, _db.LastUpdate))
                        {
                            _actualWork = true;
                            if (logger.IsEnabled(LogLevel.Information))
                            {
                                logger.LogInformation(@"Needs Refresh: {RefresherKey} @ {LastUpdate}", _cached.RefresherKey, _db.LastUpdate);
                            }
                            _cached.Refresh();

                            // notify gather
                            if (Notifiers.TryGetValue(_cached.RefresherKey, out var _notifiers)
                                && (_notifiers.Count != 0))
                            {
                                _gatherNotifiers.AddRange(_notifiers.Where(_n => !_gatherNotifiers.Contains(_n)));
                            }
                        }
                    }
                }
                else if (ludCacheFreshness.GetFreshness(_cached.RefresherKey) == null)
                {
                    // if _cached not currently tracked, must add
                    if (ludCacheFreshness.SetFreshness(_cached.RefresherKey, DateTimeOffset.Now))
                    {
                        _actualWork = true;
                        if (logger.IsEnabled(LogLevel.Information))
                        {
                            logger.LogInformation(@"No Cache: {RefresherKey} @ {Now}", _cached.RefresherKey, DateTimeOffset.Now);
                        }
                        _cached.Refresh();

                        // update shared freshness
                        var _fresh = tableFreshnessContext.TableFreshnesses.Find(_cached.RefresherKey);
                        if (_fresh == null)
                        {
                            tableFreshnessContext.TableFreshnesses.Add(new TableFreshness
                            {
                                TableKey = _cached.RefresherKey,
                                ConcurrencyCheck = [],
                                LastUpdate = DateTimeOffset.Now
                            });
                        }

                        // notify gather
                        if (Notifiers.TryGetValue(_cached.RefresherKey, out var _notifiers)
                            && _notifiers.Count != 0)
                        {
                            _gatherNotifiers.AddRange(_notifiers.Where(_n => !_gatherNotifiers.Contains(_n)));
                        }
                    }
                }
            }
        }

        // commit
        if (_actualWork)
        {
            // notify gathered notifiers
            var _notifyWork = _gatherNotifiers
                .SelectMany(_n => _n.Notify())
                .Concat([tableFreshnessContext])
                .ToList();

            workCommitter.CommitWork(_notifyWork);
        }
        else
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(@"Still fresh @ {Now}", DateTimeOffset.Now);
            }
        }
    }
}
