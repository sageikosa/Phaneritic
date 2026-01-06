using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Ledgering;

namespace Phaneritic.Implementations.Models.Ledgering;
[PrimaryKey(nameof(ActivityID), nameof(EntryIndex), nameof(InfoEntryKey))]
public class InfoEntry : CommonLedgerEntry
{
    public InfoEntryKey InfoEntryKey { get; set; }
    public InfoEntryValue InfoEntryValue { get; set; }
}
