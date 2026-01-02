using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.Database;

public static class DbSchemaNaming
{
    public static string GetSchemaName(this IEnumerable<IDbSchemaProvider> providers, string schema, string defaultName)
        => providers.Select(_n => _n.GetDbSchemaName(schema)).FirstOrDefault() ?? defaultName;
}
