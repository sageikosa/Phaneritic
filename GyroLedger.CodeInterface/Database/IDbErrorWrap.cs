namespace GyroLedger.CodeInterface.Database;

public interface IDbErrorWrap
{
    void ErrorWrap(Action action);
}
