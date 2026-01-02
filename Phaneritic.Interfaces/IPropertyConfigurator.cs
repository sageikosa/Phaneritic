using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Phaneritic.Interfaces;

public interface IPropertyConfigurator
{
    PropertiesConfigurationBuilder ConfigureProperties(ModelConfigurationBuilder builder);
}
