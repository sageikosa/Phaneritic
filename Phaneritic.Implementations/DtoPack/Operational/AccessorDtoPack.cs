using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class AccessorDtoPack(
    IPackRecord<AccessorCredential, AccessorCredentialDto> credentialPack,
    ILudDictionary<AccessGroupKey, AccessGroupDto> accessGroups
    ) : IPackRecord<Accessor, AccessorDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public AccessorDto? Pack(Accessor? model)
        => model != null
        ? new()
        {
            AccessorID = model.AccessorID,
            AccessorKey = model.AccessorKey,
            Description = model.Description,
            AccessorTypeKey = model.AccessorTypeKey,
            AccessGroups = accessGroups.Get(model.AccessGroups?.Select(_m => _m.AccessGroupKey).ToHashSet() ?? []).ToFrozenSet(),
            Credentials = credentialPack.GetFrozenSet(model.Credentials)
        }
        : null;
}
