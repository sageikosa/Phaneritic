using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Operational;
[Index(nameof(AccessGroupKey), IsUnique = true)]
public class AccessGroup
{
    [Key]
    public AccessGroupKey AccessGroupKey { get; set; }
    public DescriptionString Description { get; set; }

    public List<AccessGroupMethod>? Methods { get; set; }

    public AccessorAccessGroup? AccessorAccessGroup { get; set; }
}
