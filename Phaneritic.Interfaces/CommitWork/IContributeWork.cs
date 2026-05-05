namespace Phaneritic.Interfaces.CommitWork;

public interface IContributeWork
{
    /// <summary>
    /// Implement this method to return additional work to be committed in the transaction.
    /// </summary>
    /// <remarks>
    /// May be an empty set.
    /// </remarks>
    IEnumerable<IContributeWork> ContributeWork();

    /// <summary>
    /// Implement this method to return additional methods to be called after the transaction is committed.
    /// </summary>
    /// <remarks>
    /// May be an empty set.
    /// </remarks>
    IEnumerable<IContributeWork> ContributeAfterWork();
}
