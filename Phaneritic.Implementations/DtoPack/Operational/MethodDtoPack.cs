using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class MethodDtoPack(
    ) : IPackRecord<Method, MethodDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public MethodDto? Pack(Method? model)
        => model != null
        ? new()
        {
            MethodKey = model.MethodKey,
            Description = model.Description,
            IsTransient = model.IsTransient,
            StayWithAccessMechanism = model.StayWithAccessMechanism,
            StayWithAccessor = model.StayWithAccessor,
            ValidAccessMechanismTypeKeys = (model.AccessMechanismTypes?.Select(_m => _m.AccessMechanismTypeKey).ToHashSet() ?? []).ToFrozenSet()
        }
        : null;
}
