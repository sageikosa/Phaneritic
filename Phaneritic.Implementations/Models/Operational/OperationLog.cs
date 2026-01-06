using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Operational;
public class OperationLog
{
    [Key]
    public OperationLogID OperationLogID { get; set; }
    public AccessSessionID AccessSessionID { get; set; }
    public OperationID? OperationID { get; set; }
    public MethodKey? MethodKey { get; set; }
    public DateTimeOffset LogTime { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessorID AccessorID { get; set; }
    public bool IsComplete { get; set; }
}
