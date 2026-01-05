namespace Phaneritic.Interfaces.Operational;
public record OperationLogDto
{
    public OperationID OperationID { get; set; }
    public MethodKey MethodKey { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? EndedAt { get; set; }
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessorID AccessorID { get; set; }
}
