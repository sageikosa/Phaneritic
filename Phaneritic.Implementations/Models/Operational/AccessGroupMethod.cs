using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(AccessGroupKey), nameof(MethodKey))]
public class AccessGroupMethod
{
    public AccessGroupKey AccessGroupKey { get; set; }
    public MethodKey MethodKey { get; set; }

    [ForeignKey(nameof(AccessGroupKey))]
    public AccessGroup? AccessGroup { get; set; }

    [ForeignKey(nameof(MethodKey))] 
    public Method? Method { get; set; }
}
