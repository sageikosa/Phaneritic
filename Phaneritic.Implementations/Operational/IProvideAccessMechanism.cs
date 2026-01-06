using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

/// <summary>
/// Used to resolve access mechanism from running service call context.
/// </summary>
public interface IProvideAccessMechanism
{
    /// <summary>
    /// Priority of resolution attempt
    /// </summary>
    /// <remarks>
    /// lower number = higher priority
    /// </remarks>
    int Priority { get; }

    /// <summary>
    /// Resolved access mechanism for running service call context.
    /// </summary>
    /// <remarks>
    /// null if this instance is unable to provide
    /// </remarks>
    AccessMechanismDto? CurrentAccessMechanism { get; }
}

