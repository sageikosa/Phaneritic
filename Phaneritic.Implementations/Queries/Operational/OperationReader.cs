using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces.Operational;
using System.Collections.Frozen;

namespace Phaneritic.Implementations.Queries.Operational;

public class OperationReader(
    IEnumerable<IProvideScopedOperations> provideOperations
    ) : IOperationReader
{
    private readonly List<IProvideScopedOperations> _ProvideOperations = [.. provideOperations];

    public FrozenSet<MethodKey> GetSessionMethods()
        => _ProvideOperations.GetScopedMethods();

    public FrozenSet<OperationDto> GetSessionOperations()
        => _ProvideOperations.GetScopedOperations();
}
