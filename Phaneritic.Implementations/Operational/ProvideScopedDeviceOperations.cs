using Phaneritic.Interfaces.Operational;
using Phaneritic.Implementations.Operational;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using Phaneritic.Interfaces;

namespace Phaneritic.Implementations.Operational;

public class ProvideScopedDeviceOperations(
    IAccessSessionReader accessSessionReader,
    ICanonicalDictionary<AccessMechanismID, ConcurrentDictionary<MethodKey, OperationDto>> operations
    ) : IProvideScopedOperations
{
    public int Priority => 10;

    public FrozenSet<MethodKey>? CurrentMethods
    {
        get
        {
            if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
                && !(_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? true))
            {
                // clean canonical dictionary
                if (operations.TryGetValue(_session.AccessMechanism.AccessMechanismID)
                    is ConcurrentDictionary<MethodKey, OperationDto> _oList)
                {
                    return [.. _oList.ToArray().Select(_kvp => _kvp.Key)];
                }
            }
            return null;
        }
    }
    public FrozenSet<OperationDto>? CurrentOperations
    {
        get
        {
            if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
                && !(_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? true))
            {
                // clean canonical dictionary
                if (operations.TryGetValue(_session.AccessMechanism.AccessMechanismID)
                    is ConcurrentDictionary<MethodKey, OperationDto> _oList)
                {
                    return [.. _oList.ToArray().Select(_kvp => _kvp.Value)];
                }
            }
            return null;
        }
    }

    public void ClearCache()
    {
    }
}
