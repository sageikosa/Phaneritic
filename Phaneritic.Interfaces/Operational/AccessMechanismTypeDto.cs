using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record AccessMechanismTypeDto : ILudCacheable<AccessMechanismTypeKey>
{
    public AccessMechanismTypeKey AccessMechanismTypeKey { get; init; }
    public DescriptionString Description { get; init; }

    public bool IsUserAccess { get; init; }
    public bool IsRoamingAccess { get; init; }
    public bool IsPoolable { get; init; }
    public bool IsValidatedIPAddress { get; init; }

    public ProcessNodeTypeKey? ProcessNodeTypeKey { get; init; }

    public FrozenSet<AccessorTypeKey> ValidAccessorTypeKeys { get; init; } = [];
    public FrozenSet<MethodKey> ValidMethodKeys { get; init; } = [];
}
