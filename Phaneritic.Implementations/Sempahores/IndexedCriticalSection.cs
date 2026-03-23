using Microsoft.Extensions.Logging;

namespace Phaneritic.Implementations.Sempahores;

public sealed class IndexedCriticalSection<TKey, TBarrier> : IDisposable
    where TKey : struct, IEquatable<TKey>
    where TBarrier : SemaphoreBarrier, IIndexableSemaphoreBarrier<TKey>, new()
{
    private readonly HashSet<TKey> _Entered = [];
    private readonly ILogger<IndexedCriticalSection<TKey, TBarrier>> _Logger;
    private readonly IIndexedCriticalSectionDispenser<TKey, TBarrier> _Barriers;
    private bool _Disposed;

    public IndexedCriticalSection(
        ILogger<IndexedCriticalSection<TKey, TBarrier>> logger,
        IIndexedCriticalSectionDispenser<TKey, TBarrier> barriers
    )
    {
        _Logger = logger;

        // capture injected dispenser
        _Barriers = barriers;

        // prevents dispenser from clearing out barriers while active
        _Barriers.StartingUse();
    }

    public bool HasPassed(TKey gateID)
        => _Entered.Contains(gateID);

    public bool TryEnter(TKey index, int waitMilli, CancellationToken cancellationToken)
    {
        if (!_Entered.Contains(index))
        {
            var _blocker = _Barriers.GetOrAdd(index,
                _ndx =>
                {
                    if (_Logger.IsEnabled(LogLevel.Debug))
                    {
                        _Logger.LogDebug(@"added indexed critical section {Barrier}.{Index}", nameof(TBarrier), _ndx);
                    }
                    return new TBarrier();
                });
            if (_blocker?.Enter(waitMilli, cancellationToken) ?? false)
            {
                _Entered.Add(index);
                return true;
            }
            return false;
        }
        return true;
    }

    public bool TryEnter(TKey index, int waitMilli)
    {
        if (!_Entered.Contains(index))
        {
            var _blocker = _Barriers.GetOrAdd(index, 
                _ndx =>
                {
                    if (_Logger.IsEnabled(LogLevel.Debug))
                    {
                        _Logger.LogDebug(@"added indexed critical section {Barrier}.{Index}", nameof(TBarrier), _ndx);
                    }
                    return new TBarrier();
                });
            if (_blocker?.Enter(waitMilli) ?? false)
            {
                _Entered.Add(index);
                return true;
            }
            return false;
        }
        return true;
    }

    public bool TryEnter(HashSet<TKey> indexes, int waitMilli)
    {
        // NOTE: consider asyncing this?

        // only check those that haven't been entered yet
        var _keys = indexes.Where(_g => !_Entered.Contains(_g))
            .Distinct()
            .ToHashSet();
        if (_keys.Count != 0)
        {
            // make checks for all keys
            var _checks = (from _g in _keys
                           select Task.Run(() => TryEnter(_g, waitMilli)))
                          .ToArray();

            // all must pass
            Task.WaitAll(_checks);
            var _success = _checks.All(_t => _t.Result);

            if (!_success)
            {
                // didn't all succeed, re-open gates
                foreach (var _k in _keys)
                {
                    TryLeave(_k);
                }
            }
            return _success;
        }
        return true;
    }

    public void TryLeave(TKey index)
    {
        if (_Entered.Remove(index))
        {
            _Barriers.TryGetValue(index)?.Leave();
        }
    }

    public bool IsEnterable(TKey index)
    {
        if (!_Entered.Contains(index))
        {
            var _blocker = _Barriers.GetOrAdd(index, 
                _ndx =>            
                {
                    if (_Logger.IsEnabled(LogLevel.Debug))
                    {
                        _Logger.LogDebug(@"added indexed critical section {Barrier}.{Index}", nameof(TBarrier), _ndx);
                    }
                    return new TBarrier();
                });
            return !(_blocker?.InUse() ?? true);
        }
        return true;
    }

    public bool AreEnterable(HashSet<TKey> indexes)
    {
        // only check those that haven't been entered yet
        var _keys = indexes.Where(_g => !_Entered.Contains(_g))
            .Distinct()
            .ToHashSet();
        if (_keys.Count != 0)
        {
            // make checks for all keys
            var _checks = (from _g in _keys
                           select Task.Run(() => IsEnterable(_g)))
                          .ToArray();

            // all must pass
            Task.WaitAll(_checks);
            return _checks.All(_t => _t.Result);
        }
        return true;
    }

    private void Dispose(bool disposing)
    {
        if (!_Disposed)
        {
            if (disposing)
            {
                foreach (var _entered in _Entered)
                {
                    _Barriers.TryGetValue(_entered)?.Leave();
                }

                // allows the dispenser to clean up unused barriers
                _Barriers.FinishedUse();

                _Entered.Clear();
            }

            _Disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code
        // Put cleanup code in Dispose(bool disposing) method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
