namespace Phaneritic.Interfaces.Operational;
public record OperationDto
{
    public OperationID OperationID { get; set; }
    public MethodKey MethodKey { get; set; }
    public DateTimeOffset StartedAt { get; set; }   
    public AccessMechanismID AccessMechanismID { get; set; }
    public AccessorID AccessorID { get; set; }
}
