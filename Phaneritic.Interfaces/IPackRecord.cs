using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace Phaneritic.Interfaces;
public interface IPackRecord<in TModel, TDto>
    where TDto: class, IEquatable<TDto>
{
    [return: NotNullIfNotNull(nameof(model))]
    TDto? Pack(TModel? model);

    /// <summary>Default interface implementation using <see cref="Pack(TModel?)"/></summary>
    public IEnumerable<TDto> GetDtos(IEnumerable<TModel> models)
        => models.Select(x => Pack(x)).OfType<TDto>();

    public FrozenSet<TDto> GetFrozenSet(IEnumerable<TModel>? models)
        => models == null
        ? []
        : models.Select(x => Pack(x)).OfType<TDto>().ToFrozenSet();
}

