using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(OptionGroupKey),nameof(OptionTypeKey))]
public class OptionGroupOptionType
{
    public OptionGroupKey OptionGroupKey { get; set; }
    public OptionTypeKey OptionTypeKey { get; set; }
}
