using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record ProcessNodeTypeDto : ILudCacheable<ProcessNodeTypeKey>
{
    public ProcessNodeTypeKey ProcessNodeTypeKey { get; init; }
    public DescriptionString Description { get; init; }

    public FrozenSet<OptionGroupKey> ValidOptionGroups { get; init; } = [];
    public FrozenSet<OperationalStateTypeKey> ValidOperationalStateTypes { get; init; } = [];
}
