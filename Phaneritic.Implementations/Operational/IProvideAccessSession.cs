using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

/// <summary>
/// Used to resolve access session from running service call context.
/// </summary>
public interface IProvideAccessSession
{
    /// <summary>
    /// Priority of resolution attempt
    /// </summary>
    /// <remarks>
    /// lower number = higher priority
    /// </remarks>
    int Priority { get; }

    /// <summary>
    /// Resolved access session for running service call context.
    /// </summary>
    /// <remarks>
    /// null if this instance is unable to provide
    /// </remarks>
    AccessSessionDto? CurrentAccessSession { get; }
}
