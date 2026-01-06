using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class ProcessNodeType
{
    [Key]
    public ProcessNodeTypeKey ProcessNodeTypeKey { get; set; }
    public DescriptionString Description { get; set; }

    [ForeignKey(nameof(ProcessNodeTypeKey))]
    public List<ProcessNodeTypeOptionGroup>? OptionGroups { get; set; }
}
