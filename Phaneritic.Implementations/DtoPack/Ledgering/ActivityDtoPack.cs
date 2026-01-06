using Phaneritic.Implementations.Models.Ledgering;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Ledgering;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Ledgering;
public class ActivityDtoPack
    : IPackRecord<Activity, ActivityDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public ActivityDto? Pack(Activity? model)
        => model != null
        ? new()
        {
            ActivityID = model.ActivityID,
            AccessorID = model.AccessorID,
            AccessMechanismID = model.AccessMechanismID,
            AccessSessionID = model.AccessSessionID,
            ActivityTypeKey = model.ActivityTypeKey,
            MethodKey = model.MethodKey,
            OperationID = model.OperationID,
            StartAt = model.StartAt,
            EndAt = model.EndAt,
            DurationMicroSeconds = model.DurationMicroSeconds,
            DurationMilliSeconds = model.DurationMilliSeconds,
            IsSuccessful = model.IsSuccessful
        }
        : null;
}
