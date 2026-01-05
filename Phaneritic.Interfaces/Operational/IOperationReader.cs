using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public interface IOperationReader
{
    FrozenSet<OperationDto> GetSessionOperations();
    FrozenSet<MethodKey> GetSessionMethods();
}
