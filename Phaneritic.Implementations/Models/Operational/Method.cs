using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Operational;
public class Method
{
    [Key]
    public MethodKey MethodKey { get; set; }
    public DescriptionString Description { get; set; }

    /// <summary>When mechanism stops a session, transients are stopped</summary>
    public bool IsTransient { get; set; }

    /// <summary>If true, when mechanism stops a session, this method will stay with the mechanism.</summary>
    public bool StayWithAccessMechanism { get; set; }

    /// <summary>If true, when mechanism stops a session, this method will stay with the accessor.</summary>
    public bool StayWithAccessor { get; set; }

    public List<MethodAccessMechanismType>? AccessMechanismTypes { get; set; }
}
