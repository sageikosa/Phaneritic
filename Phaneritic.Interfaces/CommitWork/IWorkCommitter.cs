namespace Phaneritic.Interfaces.CommitWork;

public interface IWorkCommitter
{
    /// <summary>
    /// Commit all work from the list of "top-level" work contributors to process.
    /// </summary>
    /// <remarks>
    /// Recursively processes each item in the list, both for committal and after committal.
    /// </remarks>
    void CommitWork(params List<IContributeWork> commitWorks);
}
