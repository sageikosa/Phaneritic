namespace Phaneritic.Interfaces.CommitWork;

public interface IWorkCommitter
{
    void CommitWork(params List<IContributeWork> commitWorks);
}
