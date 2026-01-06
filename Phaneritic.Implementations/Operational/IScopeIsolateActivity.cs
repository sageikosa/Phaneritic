namespace Phaneritic.Implementations.Operational;

/// <summary>Isolates activity within a new scope using an explicit cloned AccessSesionDto</summary>
/// <remarks>Suitable for retry as all scoped services will be disposed upon exit.</remarks>
public interface IScopeIsolateActivity
{
    void ScopeIsolate<IService>(Action<IService> scopeActions);
}