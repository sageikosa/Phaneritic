using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Interfaces.Ledgering;
public record ActivityDto
{
    public ActivityID ActivityID { get; set; }
    public AccessorID AccessorID { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessSessionID AccessSessionID { get; set; }
    public ActivityTypeKey ActivityTypeKey { get; set; }    
    public MethodKey MethodKey { get; set; }
    public OperationID OperationID { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public long DurationMicroSeconds { get; set; }
    public long DurationMilliSeconds { get; set; }

    public bool IsSuccessful { get; set; }
}
