using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(MethodKey), nameof(AccessMechanismTypeKey))]
public class MethodAccessMechanismType
{
    public MethodKey MethodKey { get; set; }
    public AccessMechanismTypeKey AccessMechanismTypeKey { get; set; }

    [ForeignKey(nameof(MethodKey))]
    public Method? Method { get; set; }

    [ForeignKey(nameof(AccessMechanismTypeKey))]
    public AccessMechanismType? AccessMechanismType { get; set; }
}
