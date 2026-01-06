using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;

[PrimaryKey(nameof(AccessorTypeKey), nameof(AccessorCredentialTypeKey))]
public class AccessorTypeAccessorCredentialType
{
    public AccessorTypeKey AccessorTypeKey { get; set; }
    public AccessorCredentialTypeKey AccessorCredentialTypeKey { get; set; }

    [ForeignKey(nameof(AccessorTypeKey))]
    public AccessorType? AccessorType { get; set; }

    [ForeignKey(nameof(AccessorCredentialTypeKey))]
    public AccessorCredentialType? AccessorCredentialType { get; set; }
}
