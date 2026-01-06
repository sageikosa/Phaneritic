using Phaneritic.Interfaces.Operational;
using Phaneritic.Implementations.Operational;
using System.Collections.Frozen;
using Phaneritic.Implementations.Models.Operational;

namespace Phaneritic.Implementations.Operational;

public class ProvideScopedUserOperations(
    IAccessSessionReader accessSessionReader,
    IOperationalContext operationalContext
    ) : IProvideScopedOperations
{
    private FrozenSet<MethodKey>? _Methods;
    private FrozenSet<OperationDto>? _Operations;

    public int Priority => 100;

    public FrozenSet<MethodKey>? CurrentMethods
    {
        get
        {
            if (_Methods != null)
            {
                return _Methods;
            }

            if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
                && (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false))
            {
                _Methods = operationalContext.Operations
                    .Where(_o => _o.AccessSessionID == _session.AccessSessionID)
                    .Select(_o => _o.MethodKey)
                    .Distinct()
                    .ToFrozenSet();
                return _Methods;
            }
            return null;
        }
    }

    public FrozenSet<OperationDto>? CurrentOperations
    {
        get
        {
            if (_Operations != null)
            {
                return _Operations;
            }

            if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
                && (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false))
            {
                _Operations = operationalContext.Operations
                    .Where(_o => _o.AccessSessionID == _session.AccessSessionID)
                    .ToList()
                    .Select(_o => new OperationDto
                    {
                        AccessMechanismID = _o.AccessMechanismID,
                        AccessorID = _o.AccessorID,
                        MethodKey = _o.MethodKey,
                        OperationID = _o.OperationID,
                        StartedAt = _o.StartedAt,
                    })
                    .ToFrozenSet();
                return _Operations;
            }
            return null;
        }
    }

    public void ClearCache()
    {
        if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
            && (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false))
        {
            _Operations = null;
            _Methods = null;
        }
    }
}
