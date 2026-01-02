namespace Phaneritic.Interfaces.Database;

public interface IDbErrorWrap
{
    void ErrorWrap(Action action);
}
