using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record AccessorTypeDto
{
    public AccessorTypeKey AccessorTypeKey { get; init; }
    public DescriptionString Description { get; init; }
    public FrozenSet<AccessMechanismTypeKey> ValidAccessMechanismTypeKeys { get; init; } = [];
    public FrozenSet<AccessorCredentialTypeKey> ValidAccessorCredentialTypeKeys { get; init; } = [];
}
