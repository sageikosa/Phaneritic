using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;
public interface IManageOperation
{
    int Priority { get; }

    OperationDto? StartOperation(MethodKey methodKey);
    bool StopOperation(OperationID operationID);
}
