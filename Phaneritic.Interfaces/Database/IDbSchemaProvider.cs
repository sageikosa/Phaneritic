namespace Phaneritic.Interfaces.Database;

public interface IDbSchemaProvider
{
    string GetDbSchemaName(string schema);
}
