using Phaneritic.Interfaces.Operational;
using Phaneritic.Implementations.Operational;
using Phaneritic.Implementations.Models.Operational;

namespace Phaneritic.Implementations.Operational;

/// <remarks>
/// <para>User AccessSessions are tracked per DI scope.</para>
/// <para>This class fetches the AccessSession, keeping a scope-local copy for subsequent in-scope calls.</para>
/// </remarks>
public class ProvideUserAccessSession(
    IAccessMechanismReader accessMechanismReader,
    IAccessorReader accessorReader,
    IOperationalContext operationalContext
    ) : IProvideAccessSession
{
    private AccessSessionDto? _Current;

    public int Priority => 100;

    public AccessSessionDto? CurrentAccessSession
    {
        get
        {
            if (_Current == null)
            {
                var _mechanism = accessMechanismReader.GetScopedAccessMechanism();
                if (_mechanism?.AccessMechanismType.IsUserAccess ?? false)
                {
                    if (_mechanism == null)
                    {
                        // !!! FAIL-SAFE !!!
                        // shouldn't get here if all IProvideAccessMechanism instances registered with intended priority
                        throw new InvalidOperationException(@"no access mechanism");
                    }

                    // find accessor
                    var _accessor = accessorReader.GetScopedAccessor();
                    if (_accessor != null)
                    {
                        var _accessorID = _accessor?.AccessorID ?? default;
                        var _accessMechanismID = _mechanism?.AccessMechanismID ?? default;
                        var _accessSession = (from _as in operationalContext.AccessSessions
                                              where _as.AccessorID == _accessorID
                                              && _as.AccessMechanismID == _accessMechanismID
                                              select _as).FirstOrDefault();
                        if (_accessSession != null)
                        {
                            _Current = new AccessSessionDto
                            {
                                AccessSessionID = _accessSession.AccessSessionID,
                                StartedAt = _accessSession.StartedAt,
                                Accessor = _accessor,
                                AccessMechanism = _mechanism
                            };
                        }
                        else
                        {
                            // TODO: log?
                        }
                    }
                    else
                    {
                        // TODO: log?
                    }
                }
            }
            return _Current;
        }
    }
}
