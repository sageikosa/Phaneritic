namespace Phaneritic.Interfaces.Operational;

public readonly partial struct AccessorCredentialTypeKey
{
    public static readonly AccessorCredentialTypeKey HashWord = new(@"HASHWD");
    public static readonly AccessorCredentialTypeKey TokenClaim = new(@"TOKCLM");
    public static readonly AccessorCredentialTypeKey IpAddress = new(@"IPADDR");
    public static readonly AccessorCredentialTypeKey Secret = new(@"SECRET");
}
