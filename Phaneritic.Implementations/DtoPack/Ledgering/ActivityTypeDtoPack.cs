using Phaneritic.Implementations.Models.Ledgering;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Ledgering;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Ledgering;
public class ActivityTypeDtoPack
    : IPackRecord<ActivityType, ActivityTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public ActivityTypeDto? Pack(ActivityType? model)
        => model != null
        ? new()
        {
            ActivityTypeKey = model.ActivityTypeKey,
            Description = model.Description,
            Category = model.Category,
        }
        : null;
}
