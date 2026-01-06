using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Models.Operational;
[PrimaryKey(nameof(ProcessNodeKey), nameof(OptionTypeKey))]
public class Option
{
    public ProcessNodeKey ProcessNodeKey { get; set; }
    public OptionTypeKey OptionTypeKey { get; set; }

    public OptionValue OptionValue { get; set; }
}
