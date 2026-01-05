namespace Phaneritic.Interfaces.Operational;

public record AccessorCredentialTypeDto
{
    public AccessorCredentialTypeKey AccessorCredentialTypeKey { get; init; }
    public DescriptionString Description { get; init; }
}
