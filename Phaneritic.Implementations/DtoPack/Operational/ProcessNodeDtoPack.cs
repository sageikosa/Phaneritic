using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class ProcessNodeDtoPack
    : IPackRecord<ProcessNode, ProcessNodeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public ProcessNodeDto? Pack(ProcessNode? model)
        => model != null
        ? new()
        {
            ProcessNodeKey = model.ProcessNodeKey,
            Description = model.Description,
            ProcessNodeTypeKey = model.ProcessNodeTypeKey,
            ParentNodeKey = model.ParentNodeKey,
            Options = (model.Options?.ToDictionary(_o => _o.OptionTypeKey, _o => _o.OptionValue) ?? []).ToFrozenDictionary()
        }
        : null;
}
