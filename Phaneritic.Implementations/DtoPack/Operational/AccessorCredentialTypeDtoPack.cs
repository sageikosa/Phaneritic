using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessorCredentialTypeDtoPack
    : IPackRecord<AccessorCredentialType, AccessorCredentialTypeDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessorCredentialTypeDto? Pack(AccessorCredentialType? model)
        => model != null
        ? new()
        {
            AccessorCredentialTypeKey = model.AccessorCredentialTypeKey,
            Description = model.Description
        }
        : null;
}