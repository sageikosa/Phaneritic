namespace Phaneritic.Implementations.Sempahores;

/// <summary>
/// Semaphore barrier for shared resource access with timeout and cancellation.
/// </summary>
public abstract class SemaphoreBarrier
{
    private readonly SemaphoreSlim _Semaphore = new(1, 1);

    public bool InUse()
        => _Semaphore.CurrentCount == 0;


    public bool Enter(int waitMS, CancellationToken cancellationToken)
        => _Semaphore.Wait(waitMS, cancellationToken);

    public bool Enter(int waitMS)
        => _Semaphore.Wait(waitMS);

    public void Leave()
    {
        _Semaphore.Release();
    }
}
