using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phaneritic.Implementations.Models.Operational;
public class ProcessNode
{
    [Key]
    public ProcessNodeKey ProcessNodeKey { get; set; }
    public DescriptionString Description { get; set; }

    public ProcessNodeTypeKey ProcessNodeTypeKey { get; set; }
    public ProcessNodeKey? ParentNodeKey { get; set; }

    [ForeignKey(nameof(ProcessNodeTypeKey))]
    public ProcessNodeType? ProcessNodeType { get; set; }

    [ForeignKey(nameof(ParentNodeKey))]
    public ProcessNode? ParentNode { get; set; }

    [ForeignKey(nameof(ProcessNodeKey))]
    public List<Option>? Options { get; set; }
}
