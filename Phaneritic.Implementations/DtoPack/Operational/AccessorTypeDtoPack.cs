using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessorTypeDtoPack(
    ) : IPackRecord<AccessorType, AccessorTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessorTypeDto? Pack(AccessorType? model)
        => model != null
        ? new()
        {
            AccessorTypeKey = model.AccessorTypeKey,
            Description = model.Description,
            ValidAccessMechanismTypeKeys = (model.AccessMechanismTypes?.Select(_m => _m.AccessMechanismTypeKey).ToHashSet() ?? []).ToFrozenSet(),
            ValidAccessorCredentialTypeKeys= (model.AccessorCredentialTypes?.Select(_m => _m.AccessorCredentialTypeKey).ToHashSet() ?? []).ToFrozenSet(),
        }
        : null;
}
