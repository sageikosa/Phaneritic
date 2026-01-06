using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Operational;
public class AccessorType
{
    [Key]
    public AccessorTypeKey AccessorTypeKey { get; set; }
    public DescriptionString Description { get; set; }

    public List<AccessorTypeAccessMechanismType>? AccessMechanismTypes { get; set; }
    public List<AccessorTypeAccessorCredentialType>? AccessorCredentialTypes { get; set; }
}
