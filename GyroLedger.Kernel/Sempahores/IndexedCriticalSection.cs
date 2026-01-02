using GyroLedger.CodeInterface;
using Microsoft.Extensions.Logging;

namespace GyroLedger.Kernel.Sempahores;

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
