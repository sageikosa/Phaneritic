using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Queries.Operational;
public class AccessSessionReader(
    IOperationalContext operationalContext,
    IAccessMechanismReader accessMechanismReader,
    IAccessorReader accessorReader,
    IEnumerable<IProvideAccessSession> provideAccessSessions
    ) : IAccessSessionReader
{
    public AccessSessionDto? GetAccessSession(AccessSessionID accessSessionID)
    {
        var _session = operationalContext.AccessSessions.FirstOrDefault(_as => _as.AccessSessionID == accessSessionID);
        if (_session != null)
        {
            return new AccessSessionDto
            {
                AccessSessionID = _session.AccessSessionID,
                StartedAt = _session.StartedAt,
                Accessor = accessorReader.GetAccessor(_session.AccessorID),
                AccessMechanism = accessMechanismReader.GetAccessMechanism(_session.AccessMechanismID)
            };
        }
        return null;
    }

    public AccessSessionDto? GetAccessSession(AccessMechanismID accessMechanismID)
    {
        var _session = operationalContext.AccessSessions.FirstOrDefault(_as => _as.AccessMechanismID == accessMechanismID);
        if (_session != null)
        {
            return new AccessSessionDto
            {
                AccessSessionID = _session.AccessSessionID,
                StartedAt = _session.StartedAt,
                Accessor = accessorReader.GetAccessor(_session.AccessorID),
                AccessMechanism = accessMechanismReader.GetAccessMechanism(_session.AccessMechanismID)
            };
        }
        return null;
    }

    public AccessSessionDto? GetAccessSession(AccessMechanismKey accessMechanismKey)
    {
        var _session = operationalContext.AccessSessions.FirstOrDefault(_as => _as.AccessMechanism!.AccessMechanismKey == accessMechanismKey);
        if (_session != null)
        {
            return new AccessSessionDto
            {
                AccessSessionID = _session.AccessSessionID,
                StartedAt = _session.StartedAt,
                Accessor = accessorReader.GetAccessor(_session.AccessorID),
                AccessMechanism = accessMechanismReader.GetAccessMechanism(_session.AccessMechanismID)
            };
        }
        return null;
    }

    public List<AccessSessionDto> GetAccessSessions(AccessorID accessorID)
    {
        if (accessorReader.GetAccessor(accessorID) is AccessorDto _accessor)
        {
            return [..operationalContext.AccessSessions.Where(_as => _as.AccessorID == accessorID).ToList()
                .Select(_as => new AccessSessionDto
                {
                    Accessor = _accessor,
                    StartedAt = _as.StartedAt,
                    AccessSessionID = _as.AccessSessionID,
                    AccessMechanism = accessMechanismReader.GetAccessMechanism(_as.AccessMechanismID)
                })];
        }
        return [];
    }

    public AccessSessionDto? GetScopedAccessSession()
        => provideAccessSessions.GetScopedAccessSession();
}
