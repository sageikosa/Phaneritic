namespace Phaneritic.Interfaces.CommitWork;

public interface IContributeWork
{
    IEnumerable<IContributeWork> ContributeWork();
    IEnumerable<IContributeWork> ContributeAfterWork();
}
