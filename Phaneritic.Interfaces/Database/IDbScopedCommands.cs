namespace Phaneritic.Interfaces.Database;

public interface IDbScopedCommands : IBaseDbCommands
{
}

public interface IDbTransientCommands : IDbScopedCommands
{
}
