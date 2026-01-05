namespace Phaneritic.Interfaces.Operational;
public record AccessSessionDto
{
    public AccessSessionID AccessSessionID { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public AccessorDto? Accessor { get; set; }
    public AccessMechanismDto? AccessMechanism { get; set; }
}
