using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace GyroLedger.CodeInterface;

public abstract class ConfigureIDBase<TStrong, TValue>(
    Expression<Func<TStrong, TValue>> convertToProviderExpression,
    Expression<Func<TValue, TStrong>> convertFromProviderExpression
    )
    : ValueConverter<TStrong, TValue>(
        convertToProviderExpression,
        convertFromProviderExpression
        ), IPropertyConfigurator
    where TStrong : struct
    where TValue : struct
{
    public Type ConvertType => typeof(TStrong);
    public Type Converter => GetType();

    public PropertiesConfigurationBuilder ConfigureProperties(ModelConfigurationBuilder builder)
        => builder
        .Properties<TStrong>()
        .HaveConversion(GetType());
}
