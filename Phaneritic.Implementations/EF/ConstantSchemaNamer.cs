using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.EF;

public class ConstantSchemaNamer(string schema) : IDbSchemaProvider
{
    public string GetDbSchemaName(string schemaKey)
        => schema;
}
