using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class OptionType
{
    [Key]
    public OptionTypeKey OptionTypeKey { get; set; }
    public DescriptionString Description { get; set; }

    [ForeignKey(nameof(OptionTypeKey))]
    public List<OptionGroupOptionType>? OptionGroups { get; set; }
}
