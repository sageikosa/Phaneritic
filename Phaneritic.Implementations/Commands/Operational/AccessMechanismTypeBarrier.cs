using Phaneritic.Implementations.Sempahores;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Commands.Operational;

/// <summary>
/// Semaphore barrier indexed by access mechanism type
/// </summary>
public class AccessMechanismTypeBarrier : SemaphoreBarrier, IIndexableSemaphoreBarrier<AccessMechanismTypeKey>
{
}
