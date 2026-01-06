using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(AccessorTypeKey), nameof(AccessMechanismTypeKey))]
public class AccessorTypeAccessMechanismType
{
    public AccessorTypeKey AccessorTypeKey { get; set; }
    public AccessMechanismTypeKey AccessMechanismTypeKey { get; set; }

    [ForeignKey(nameof(AccessorTypeKey))]
    public AccessorType? AccessorType { get; set; }

    [ForeignKey(nameof(AccessMechanismTypeKey))]
    public AccessMechanismType? AccessMechanismType { get; set; }
}
