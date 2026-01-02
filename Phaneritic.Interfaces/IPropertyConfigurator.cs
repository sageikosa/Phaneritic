using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyroLedger.CodeInterface;

public interface IPropertyConfigurator
{
    PropertiesConfigurationBuilder ConfigureProperties(ModelConfigurationBuilder builder);
}
