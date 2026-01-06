using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[Index(nameof(AccessMechanismKey), IsUnique = true)]
public class AccessMechanism
{
    [Key]
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessMechanismKey AccessMechanismKey { get; set; }

    public DescriptionString Description { get; set; }

    public ProcessNodeKey? ProcessNodeKey { get; set; }
    public bool IsEnabled { get; set; }

    public AccessMechanismTypeKey AccessMechanismTypeKey { get; set; }

    [ForeignKey(nameof(AccessMechanismTypeKey))]
    public AccessMechanismType? AccessMechanismType { get; set; }

    [ForeignKey(nameof(ProcessNodeKey))]
    public ProcessNode? ProcessNode { get; set; }
}
