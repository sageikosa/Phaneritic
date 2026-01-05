namespace Phaneritic.Interfaces.Ledgering;
public record InfoEntryParam
{
    public InfoEntryKey InfoEntryKey { get; set; }
    public InfoEntryValue InfoEntryValue { get; set; }
}
