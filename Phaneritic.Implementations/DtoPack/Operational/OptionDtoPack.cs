using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Implementations.DtoPack.Operational;
public class OptionDtoPack
    : IPackRecord<Option, OptionDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    public OptionDto? Pack(Option? model)
        => model != null
        ? new()
        {
            ProcessNodeKey = model.ProcessNodeKey,
            OptionTypeKey = model.OptionTypeKey,
            OptionValue = model.OptionValue,
        }
        : null;
}
