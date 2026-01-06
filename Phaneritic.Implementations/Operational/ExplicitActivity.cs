using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

public class ExplicitActivity(
    ) : IExplicitActivity
{
    private AccessSessionDto? _Session;

    public AccessSessionID AccessSessionID => _Session?.AccessSessionID ?? default;
    public DateTimeOffset StartedAt => _Session?.StartedAt ?? DateTimeOffset.MinValue;
    public AccessorDto? Accessor => _Session?.Accessor;
    public AccessMechanismDto? AccessMechanism => _Session?.AccessMechanism;

    public void SetExplicit(AccessSessionDto accessSession)
        => _Session = accessSession;
}
