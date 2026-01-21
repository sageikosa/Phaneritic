using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Phaneritic.Interfaces;

public abstract class ConfigureKeyBase<TStrong>(
    Expression<Func<TStrong, string>> convertToProviderExpression,
    Expression<Func<string, TStrong>> convertFromProviderExpression,
    int maxLength
    ) : ValueConverter<TStrong, string>(
        convertToProviderExpression,
        convertFromProviderExpression
        ), IPropertyCollation
    where TStrong : struct
{
    public virtual bool IsCaseSensitive => false;
    public virtual bool IsUnicode => false;

    public string Collation { get; set; } = string.Empty;

    public PropertiesConfigurationBuilder ConfigureProperties(ModelConfigurationBuilder builder)
        => string.IsNullOrWhiteSpace(Collation)
        ? builder
        .Properties<TStrong>()
        .AreUnicode(IsUnicode)
        .HaveMaxLength(maxLength)
        .HaveConversion(GetType())
        : builder
        .Properties<TStrong>()
        .AreUnicode(IsUnicode)
        .HaveMaxLength(maxLength)
        .HaveConversion(GetType())
        .HaveAnnotation(RelationalAnnotationNames.Collation, Collation);
}
