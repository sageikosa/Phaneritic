using Phaneritic.Interfaces.LudCache;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record ProcessNodeDto : ILudCacheable<ProcessNodeKey>
{
    public ProcessNodeKey ProcessNodeKey { get; init; }
    public DescriptionString Description { get; init; }

    public ProcessNodeTypeKey ProcessNodeTypeKey { get; init; }
    public ProcessNodeKey? ParentNodeKey { get; init; }

    public FrozenDictionary<OptionTypeKey, OptionValue> Options { get; init; } = FrozenDictionary<OptionTypeKey, OptionValue>.Empty;
}
