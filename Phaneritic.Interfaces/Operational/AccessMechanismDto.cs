using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Interfaces.Operational;
public record AccessMechanismDto : ILudCacheable<AccessMechanismID>, ILudCacheable<AccessMechanismKey>
{
    public AccessMechanismID AccessMechanismID { get; init; }
    public AccessMechanismKey AccessMechanismKey { get; init; }

    public DescriptionString Description { get; init; }

    public ProcessNodeKey? ProcessNode { get; init; }

    public bool IsEnabled { get; init; }

    public required AccessMechanismTypeDto AccessMechanismType { get; init; }
}
