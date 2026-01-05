using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record OptionTypeDto : ILudCacheable<OptionTypeKey>
{
    public OptionTypeKey OptionTypeKey { get; init; }
    public DescriptionString Description { get; init; }

    public FrozenSet<OptionGroupKey> ValidOptionGroups { get; init; } = [];
}
