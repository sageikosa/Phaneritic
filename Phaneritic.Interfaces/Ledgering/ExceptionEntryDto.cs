using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Interfaces.Ledgering;
public record ExceptionEntryDto
{
    public ActivityID ActivityID { get; set; }
    public int EntryIndex { get; set; }

    public AccessorID AccessorID { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessSessionID AccessSessionID { get; set; }
    public ActivityTypeKey ActivityTypeKey { get; set; }
    public MethodKey MethodKey { get; set; }
    public OperationID OperationID { get; set; }

    public DateTimeOffset RecordedAt { get; set; }
    public long OffsetMicroSeconds { get; set; }
    public long OffsetMilliSeconds { get; set; }

    public NameString ExceptionName { get; set; }
    public DescriptionString Message { get; set; }
    public string? StackTrace { get; set; }
}
