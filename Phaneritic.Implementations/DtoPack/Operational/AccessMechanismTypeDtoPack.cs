using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessMechanismTypeDtoPack(
    ) : IPackRecord<AccessMechanismType, AccessMechanismTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessMechanismTypeDto? Pack(AccessMechanismType? model)
        => model != null
        ? new()
        {
            AccessMechanismTypeKey = model.AccessMechanismTypeKey,
            Description = model.Description,
            IsUserAccess = model.IsUserAccess,
            IsRoamingAccess = model.IsRoamingAccess,
            IsValidatedIPAddress = model.IsValidatedIPAddress,
            ProcessNodeTypeKey = model.ProcessNodeTypeKey,
            ValidAccessorTypeKeys = (model.AccessorTypes?.Select(_m => _m.AccessorTypeKey).ToHashSet() ?? []).ToFrozenSet(),
            ValidMethodKeys = (model.Methods?.Select(_m => _m.MethodKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
}
