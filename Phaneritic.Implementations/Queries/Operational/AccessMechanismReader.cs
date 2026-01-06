using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Queries.Operational;
public class AccessMechanismReader(
    ILudDictionary<AccessMechanismID, AccessMechanismDto> mechanismsByID,
    ILudDictionary<AccessMechanismKey, AccessMechanismDto> mechanismsByKey,
    IEnumerable<IProvideAccessMechanism> provideAccessMechanisms
    ) : IAccessMechanismReader
{
    public AccessMechanismDto? GetAccessMechanism(AccessMechanismID accessMechanismID)
        => mechanismsByID.Get(accessMechanismID);

    public AccessMechanismDto? GetAccessMechanism(AccessMechanismKey accessMechanismKey)
        => mechanismsByKey.Get(accessMechanismKey);

    public AccessMechanismDto? GetScopedAccessMechanism()
        => provideAccessMechanisms.GetScopedAccessMechanism();
}
