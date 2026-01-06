using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;

namespace Phaneritic.Implementations.Operational;
public interface IProvideScopedOperations
{
    int Priority { get; }
    FrozenSet<MethodKey>? CurrentMethods { get; }
    FrozenSet<OperationDto>? CurrentOperations { get; }
    void ClearCache();
}
