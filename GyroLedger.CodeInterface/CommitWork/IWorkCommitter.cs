namespace GyroLedger.CodeInterface.CommitWork;

public interface IWorkCommitter
{
    void CommitWork(params List<IContributeWork> commitWorks);
}
