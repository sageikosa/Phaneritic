using Phaneritic.Interfaces.Operational;
using Phaneritic.Interfaces;

namespace Phaneritic.Implementations.Operational;

/// <remarks>
/// <para>Cached AccessSessions are cache managed outside this class.</para>
/// <para>This class just tries to use the cache for lookup if applicable for the AccessMechanism type.</para>
/// </remarks>
public class ProvideCachedAccessSession(
    ICanonicalDictionary<AccessMechanismID, AccessSessionDto> canonicalSessions,
    IAccessMechanismReader accessMechanismReader
    ) : IProvideAccessSession
{
    private AccessSessionDto? _Current;

    public int Priority => 75;

    public AccessSessionDto? CurrentAccessSession
    {
        get
        {
            if (_Current == null)
            {
                // if access mechanism not found, treat it like a cached one
                var _mechanism = accessMechanismReader.GetScopedAccessMechanism();
                if (!(_mechanism?.AccessMechanismType.IsUserAccess ?? false))
                {
                    if (_mechanism == null)
                    {
                        // FAIL
                        throw new InvalidOperationException(@"no access mechanism");
                    }

                    // fetch
                    // TODO: consider allowing additional SerialNumber claim security to check against exact AccessSessionID
                    _Current = canonicalSessions.TryGetValue(_mechanism?.AccessMechanismID ?? default)
                        ?? throw new InvalidOperationException(@"no access session"); ;
                }
            }
            return _Current;
        }
    }
}
