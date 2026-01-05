using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record MethodDto : ILudCacheable<MethodKey>
{
    public MethodKey MethodKey { get; init; }
    public DescriptionString Description { get; init; }

    /// <summary>When mechanism stops a session, transients are stopped</summary>
    public bool IsTransient { get; init; }

    /// <summary>If true, when mechanism stops a session, this method will stay with the mechanism.</summary>
    public bool StayWithAccessMechanism { get; init; }

    /// <summary>If true, when mechanism stops a session, this method will stay with the accessor.</summary>
    public bool StayWithAccessor { get; init; }

    public FrozenSet<AccessMechanismTypeKey> ValidAccessMechanismTypeKeys { get; init; } = [];
}
