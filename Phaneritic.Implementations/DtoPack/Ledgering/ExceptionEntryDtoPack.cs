using Phaneritic.Implementations.Models.Ledgering;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Ledgering;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Ledgering;
public class ExceptionEntryDtoPack
    : IPackRecord<ExceptionEntry, ExceptionEntryDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public ExceptionEntryDto? Pack(ExceptionEntry? model)
        => model != null
        ? new()
        {
            ActivityID = model.ActivityID,
            EntryIndex = model.EntryIndex,
            AccessorID = model.AccessorID,
            AccessMechanismID = model.AccessMechanismID,
            AccessSessionID = model.AccessSessionID,
            ActivityTypeKey = model.ActivityTypeKey,
            MethodKey = model.MethodKey,
            OperationID = model.OperationID,
            RecordedAt = model.RecordedAt,
            OffsetMicroSeconds = model.OffsetMicroSeconds,
            OffsetMilliSeconds = model.OffsetMilliSeconds,
            ExceptionName = model.ExceptionName,
            Message = model.Message,
            StackTrace = model.StackTrace
        }
        : null;
}
