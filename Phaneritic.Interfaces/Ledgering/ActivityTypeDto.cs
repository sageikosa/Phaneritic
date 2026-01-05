using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Interfaces.Ledgering;
public record ActivityTypeDto : ILudCacheable<ActivityTypeKey>
{
    public ActivityTypeKey ActivityTypeKey { get; init; }
    public DescriptionString Description { get; init; }
    public ActivityCategory Category { get; init; }
}
