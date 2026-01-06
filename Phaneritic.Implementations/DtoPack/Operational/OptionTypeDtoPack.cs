using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class OptionTypeDtoPack
    : IPackRecord<OptionType, OptionTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public OptionTypeDto? Pack(OptionType? model)
        => model != null
        ? new()
        {
            OptionTypeKey = model.OptionTypeKey,
            Description = model.Description,
            ValidOptionGroups = (model.OptionGroups?.Select(_og => _og.OptionGroupKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
}
