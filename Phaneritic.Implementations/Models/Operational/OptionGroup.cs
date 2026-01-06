using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class OptionGroup
{
    [Key]
    public OptionGroupKey OptionGroupKey { get; set; }
    public DescriptionString Description { get; set; }

    [ForeignKey(nameof(OptionGroupKey))]    
    public List<OptionGroupOptionType>? OptionTypes { get; set; }
}
