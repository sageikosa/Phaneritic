using Microsoft.EntityFrameworkCore;
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
        ), IPropertyConfigurator
    where TStrong : struct
{
    public virtual bool IsUnicode => false;

    public PropertiesConfigurationBuilder ConfigureProperties(ModelConfigurationBuilder builder)
        => builder
        .Properties<TStrong>()
        .AreUnicode(IsUnicode)
        .HaveMaxLength(maxLength)
        .HaveConversion(GetType());
}
