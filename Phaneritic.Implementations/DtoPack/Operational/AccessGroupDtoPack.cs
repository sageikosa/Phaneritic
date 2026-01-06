using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessGroupDtoPack(
    ) : IPackRecord<AccessGroup, AccessGroupDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessGroupDto? Pack(AccessGroup? model)
        => model != null
        ? new()
        {
            AccessGroupKey = model.AccessGroupKey,
            Description = model.Description,
            AccessibleMethodKeys = (model.Methods?.Select(_m => _m.MethodKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
}
