using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessorCredentialDtoPack
    : IPackRecord<AccessorCredential, AccessorCredentialDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessorCredentialDto? Pack(AccessorCredential? model)
        => model != null
        ? new()
        {
            AccessorID = model.AccessorID,
            AccessorCredentialTypeKey = model.AccessorCredentialTypeKey,
            CredentialValue = model.CredentialValue,
            IsEnabled = model.IsEnabled
        }
        : null;
}