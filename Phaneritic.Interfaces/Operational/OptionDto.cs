namespace Phaneritic.Interfaces.Operational;
public record OptionDto
{
    public ProcessNodeKey ProcessNodeKey { get; init; }
    public OptionTypeKey OptionTypeKey { get; init; }

    public OptionValue OptionValue { get; init; }
}
