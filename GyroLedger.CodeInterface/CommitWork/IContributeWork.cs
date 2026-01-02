namespace GyroLedger.CodeInterface.CommitWork;

public interface IContributeWork
{
    IEnumerable<IContributeWork> ContributeWork();
    IEnumerable<IContributeWork> ContributeAfterWork();
}
