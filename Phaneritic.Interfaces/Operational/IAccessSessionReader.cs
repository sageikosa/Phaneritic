namespace Phaneritic.Interfaces.Operational;
public interface IAccessSessionReader
{
    AccessSessionDto? GetScopedAccessSession();
    AccessSessionDto? GetAccessSession(AccessSessionID accessSessionID);
    AccessSessionDto? GetAccessSession(AccessMechanismID accessMechanismID);
    AccessSessionDto? GetAccessSession(AccessMechanismKey accessMechanismKey);
    List<AccessSessionDto> GetAccessSessions(AccessorID accessorID);
}
