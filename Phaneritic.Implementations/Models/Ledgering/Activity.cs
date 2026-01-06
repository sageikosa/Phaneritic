using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Ledgering;
public class Activity
{
    [Key]
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
    public int EntryCount { get; set; }
    public int InfoEntryCount { get; set; }
    public int ExceptionEntryCount { get; set; }

    [ForeignKey(nameof(ActivityTypeKey))]
    public ActivityType? ActivityType { get; set; }

    public List<InfoEntry>? InfoEntries { get; set; }
    public List<ExceptionEntry>? ExceptionEntries { get; set; }
}
