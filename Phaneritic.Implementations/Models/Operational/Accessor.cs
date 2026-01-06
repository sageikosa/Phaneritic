using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[Index(nameof(AccessorKey))]
public class Accessor
{
    [Key]
    public AccessorID AccessorID { get; set; }
    public AccessorKey AccessorKey { get; set; }
    public DescriptionString Description { get; set; }
    public AccessorTypeKey AccessorTypeKey { get; set; }

    [ForeignKey(nameof(AccessorTypeKey))]
    public AccessorType? AccessorType { get; set; }

    public List<AccessorAccessGroup>? AccessGroups { get; set; }
    public List<AccessorCredential>? Credentials { get; set; }
}
