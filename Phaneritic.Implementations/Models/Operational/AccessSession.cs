using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class AccessSession
{
    [Key]
    public AccessSessionID AccessSessionID { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public AccessorID AccessorID { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }

    [ForeignKey(nameof(AccessMechanismID))]
    public AccessMechanism? AccessMechanism { get; set; }

    [ForeignKey(nameof(AccessorID))]
    public Accessor? Accessor { get; set; }
}
