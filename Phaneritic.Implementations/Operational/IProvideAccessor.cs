using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;
public interface IProvideAccessor
{
    int Priority { get; }
    AccessorDto? CurrentAccessor { get; }
}
