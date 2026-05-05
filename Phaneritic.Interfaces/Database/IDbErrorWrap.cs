namespace Phaneritic.Interfaces.Database;

public interface IDbErrorWrap
{
    Task ErrorWrap(Func<Task> action, CancellationToken cancellationToken);
}
