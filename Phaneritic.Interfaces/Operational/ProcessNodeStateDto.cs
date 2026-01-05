namespace Phaneritic.Interfaces.Operational;
public record ProcessNodeStateDto
{
    public ProcessNodeKey ProcessNodeKey { get; init; }
    public OperationalStateTypeKey OperationalStateTypeKey { get; init; }

    public OperationalStateKey OperationalStateKey { get; init; }
}
