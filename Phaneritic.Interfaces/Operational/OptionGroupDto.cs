using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record OptionGroupDto : ILudCacheable<OptionGroupKey>
{
    public OptionGroupKey OptionGroupKey { get; init; }
    public DescriptionString Description { get; init; }

    public FrozenSet<OptionTypeKey> ValidOptionTypeKeys { get; init; } = [];
}
