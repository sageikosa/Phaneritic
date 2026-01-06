using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces;

namespace Phaneritic.Implementations.Models.Ledgering;
[PrimaryKey(nameof(ActivityID), nameof(EntryIndex))]
public class ExceptionEntry : CommonLedgerEntry
{
    public NameString ExceptionName { get; set; }
    public DescriptionString Message { get; set; }
    public string? StackTrace { get; set; }
}
