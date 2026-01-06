using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Ledgering;
using System.ComponentModel.DataAnnotations;

namespace Phaneritic.Implementations.Models.Ledgering;
public class ActivityType
{
    [Key]
    public ActivityTypeKey ActivityTypeKey { get; set; }
    public DescriptionString Description { get; set; }
    public ActivityCategory Category { get; set; }
}
