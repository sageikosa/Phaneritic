using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(AccessorID), nameof(AccessorCredentialTypeKey))]
public class AccessorCredential
{
    public AccessorID AccessorID { get; set; }
    public AccessorCredentialTypeKey AccessorCredentialTypeKey { get; set; }

    public CredentialValue CredentialValue { get; set; }
    public bool IsEnabled { get; set; }

    [ForeignKey(nameof(AccessorID))]
    public Accessor? Accessor { get; set; }

    [ForeignKey(nameof(AccessorCredentialTypeKey))]
    public AccessorCredentialType? AccessorCredentialType { get; set; }
}