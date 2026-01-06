using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(AccessorID), nameof(AccessGroupKey))]
public class AccessorAccessGroup
{
    public AccessorID AccessorID { get; set; }
    public AccessGroupKey AccessGroupKey { get; set; }

    [ForeignKey(nameof(AccessorID))] 
    public Accessor? Accessor { get; set; }

    [ForeignKey(nameof(AccessGroupKey))]
    public AccessGroup? AccessGroup { get; set; }
}
