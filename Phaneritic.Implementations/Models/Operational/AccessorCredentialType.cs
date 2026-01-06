using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Operational;
public class AccessorCredentialType
{
    [Key]
    public AccessorCredentialTypeKey AccessorCredentialTypeKey { get; set; }
    public DescriptionString Description { get; set; }
}