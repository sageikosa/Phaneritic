namespace Phaneritic.Interfaces.Operational;

public record AccessorCredentialDto
{
    public AccessorID AccessorID { get; init; }
    public AccessorCredentialTypeKey AccessorCredentialTypeKey { get; init; }
    public CredentialValue CredentialValue { get; init; }
    public bool IsEnabled { get; init; }
}
