namespace Phaneritic.Interfaces;

/// <summary>Initialize a new DI scope to perform an action or task</summary>
public interface IScopeAction
{
    void DoAction<TActionService>(Action<TActionService> action) where TActionService : notnull;
    Task DoActionAsync<TActionService>(Func<TActionService, Task> task) where TActionService : notnull;
}