using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;
public interface IExplicitActivity
{
    void SetExplicit(AccessSessionDto accessSession);

    AccessSessionID AccessSessionID { get; }
    DateTimeOffset StartedAt { get; }
    AccessorDto? Accessor { get; }
    AccessMechanismDto? AccessMechanism { get; }
}
