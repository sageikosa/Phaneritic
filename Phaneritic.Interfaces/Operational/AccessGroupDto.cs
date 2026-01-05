using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record AccessGroupDto : ILudCacheable<AccessGroupKey>
{
    public AccessGroupKey AccessGroupKey { get; init; }
    public DescriptionString Description { get; init; }

    public FrozenSet<MethodKey> AccessibleMethodKeys { get; init; } = [];
}
