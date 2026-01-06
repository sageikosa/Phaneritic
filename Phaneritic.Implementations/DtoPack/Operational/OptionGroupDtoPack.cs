using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class OptionGroupDtoPack
    : IPackRecord<OptionGroup, OptionGroupDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public OptionGroupDto? Pack(OptionGroup? model)
        => model != null
        ? new()
        {
            OptionGroupKey = model.OptionGroupKey,
            Description = model.Description,
            ValidOptionTypeKeys = (model.OptionTypes?.Select(_ot => _ot.OptionTypeKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
}
