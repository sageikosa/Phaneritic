using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[Index(nameof(AccessMechanismID), nameof(AccessorID))]
[Index(nameof(AccessSessionID))]
public class Operation
{
    [Key]
    public OperationID OperationID { get; set; }
    public AccessSessionID AccessSessionID { get; set; }
    public MethodKey MethodKey { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessorID AccessorID { get; set; }

    [ForeignKey(nameof(MethodKey))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Method? Method { get; set; }

    [ForeignKey(nameof(AccessMechanismID))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public AccessMechanism? AccessMechanism { get; set; }

    [ForeignKey(nameof(AccessorID))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Accessor? Accessor { get; set; }

    [ForeignKey(nameof(AccessSessionID))]
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public AccessSession? AccessSession { get; set; }
}
