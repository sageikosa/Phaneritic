using GyroLedger.CodeInterface;
using Microsoft.Extensions.DependencyInjection;

namespace GyroLedger.Kernel;

public class ScopeAction(
    IServiceProvider serviceProvider
    ) : IScopeAction
{
    public void DoAction<TActionService>(Action<TActionService> action)
        where TActionService : notnull
    {
        using var _scope = serviceProvider.CreateScope();
        var _svc = _scope.ServiceProvider.GetRequiredService<TActionService>();
        action?.Invoke(_svc);
    }

    public async Task DoActionAsync<TActionService>(Func<TActionService, Task> task)
        where TActionService : notnull
    {
        using var _scope = serviceProvider.CreateScope();
        var _svc = _scope.ServiceProvider.GetRequiredService<TActionService>();
        await task(_svc);
    }
}
