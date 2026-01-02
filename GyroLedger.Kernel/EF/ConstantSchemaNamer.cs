using GyroLedger.CodeInterface.Database;

namespace GyroLedger.Kernel.EF;

public class ConstantSchemaNamer(string schema) : IDbSchemaProvider
{
    public string GetDbSchemaName(string schemaKey)
        => schema;
}
