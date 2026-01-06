using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Commands.Operational;
public interface IManageAccessSession 
{
    int Priority { get; }
    AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismKey accessMechanismKey);
    AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismTypeKey accessMechanismTypeKey);
    bool StopAccessSession(AccessSessionID accessSessionID);
}
