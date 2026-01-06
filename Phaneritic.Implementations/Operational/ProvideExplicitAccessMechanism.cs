using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;
public class ProvideExplicitAccessMechanism(
    IExplicitActivity explicitActivity
    ) : IProvideAccessMechanism
{
    public int Priority => 50;

    public AccessMechanismDto? CurrentAccessMechanism 
        => explicitActivity.AccessMechanism;
}
