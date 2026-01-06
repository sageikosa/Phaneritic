using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class AccessMechanismType
{
    [Key]
    public AccessMechanismTypeKey AccessMechanismTypeKey { get; set; }
    public DescriptionString Description { get; set; }

    public bool IsUserAccess { get; set; }  
    public bool IsRoamingAccess { get; set; }
    public bool IsPoolable { get; set; }
    public bool IsValidatedIPAddress { get; set; }

    public ProcessNodeTypeKey? ProcessNodeTypeKey { get; set; }

    public List<AccessorTypeAccessMechanismType>? AccessorTypes { get; set; }
    public List<MethodAccessMechanismType>? Methods { get; set; }
    
    [ForeignKey(nameof(ProcessNodeTypeKey))]
    public ProcessNodeType? ProcessNodeType { get; set; }
}
