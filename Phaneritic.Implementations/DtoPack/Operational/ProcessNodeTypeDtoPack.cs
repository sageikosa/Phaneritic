using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class ProcessNodeTypeDtoPack(
    ) : IPackRecord<ProcessNodeType, ProcessNodeTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public ProcessNodeTypeDto? Pack(ProcessNodeType? model)
        => model != null
        ? new()
        {
            ProcessNodeTypeKey = model.ProcessNodeTypeKey,
            Description = model.Description,

            ValidOptionGroups = (model.OptionGroups?.Select(_og => _og.OptionGroupKey).ToHashSet() ?? []).ToFrozenSet(),
        }
        : null;
}
