using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(ProcessNodeTypeKey), nameof(OptionGroupKey))]
public class ProcessNodeTypeOptionGroup
{
    public ProcessNodeTypeKey ProcessNodeTypeKey { get; set; }
    public OptionGroupKey OptionGroupKey { get; set; }

    public ProcessNodeType? ProcessNodeType { get; set; }

    [ForeignKey(nameof(OptionGroupKey))]
    public OptionGroup? OptionGroup { get; set; }
}
