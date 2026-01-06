using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessMechanismDtoPack(
    ILudDictionary<AccessMechanismTypeKey, AccessMechanismTypeDto> accessMechanisms
    ) : IPackRecord<AccessMechanism, AccessMechanismDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessMechanismDto? Pack(AccessMechanism? model)
        => model != null
        ? new()
        {
            AccessMechanismID = model.AccessMechanismID,
            AccessMechanismKey = model.AccessMechanismKey,
            AccessMechanismType = accessMechanisms.Get(model.AccessMechanismTypeKey)
                ?? new AccessMechanismTypeDto { AccessMechanismTypeKey = model.AccessMechanismTypeKey },
            Description = model.Description,
            ProcessNode = model.ProcessNodeKey,
            IsEnabled = model.IsEnabled
        }
        : null;
}
