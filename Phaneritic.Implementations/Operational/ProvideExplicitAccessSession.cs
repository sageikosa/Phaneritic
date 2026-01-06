using Phaneritic.Interfaces.Operational;
using Phaneritic.Implementations.Operational;

namespace Phaneritic.Implementations.Operational;
public class ProvideExplicitAccessSession(
    IExplicitActivity explicitActivity
    ) : IProvideAccessSession
{
    private AccessSessionDto? _Current;

    public int Priority => 50;

    public AccessSessionDto? CurrentAccessSession
    {
        get
        {
            if (_Current == null && explicitActivity.AccessSessionID != default)
            {
                _Current = new AccessSessionDto
                {
                    AccessMechanism = explicitActivity.AccessMechanism,
                    Accessor = explicitActivity.Accessor,
                    AccessSessionID = explicitActivity.AccessSessionID,
                    StartedAt = explicitActivity.StartedAt
                };
            }
            return _Current;
        }
    }
}
