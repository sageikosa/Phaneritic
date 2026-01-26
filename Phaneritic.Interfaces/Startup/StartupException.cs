namespace Phaneritic.Interfaces.Startup;

public class StartupException(
    string message,
    int cycles,
    List<IKickStart> unstarted
    ) : ApplicationException(message)
{
    public int Cycles { get; private set; } = cycles;
    public List<IKickStart> Unstarted { get; private set; } = unstarted;
}
