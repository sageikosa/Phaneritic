using Microsoft.Extensions.DependencyInjection;
using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

public class ScopeIsolateActivity(
    IServiceProvider serviceProvider,
    IAccessSessionReader accessSessionReader,
    ILedgerScribbler ledgerScribbler
    ) : IScopeIsolateActivity
{
    public void ScopeIsolate<IService>(Action<IService> scopeActions)
    {
        // create scope
        using var _scope = serviceProvider.CreateScope();

        // link activity
        var _session = accessSessionReader.GetScopedAccessSession()
            ?? throw new InvalidOperationException(@"no access session");
        var _explicit = _scope.ServiceProvider.GetRequiredService<IExplicitActivity>();
        _explicit.SetExplicit(_session);

        // link ledgering
        _scope.ServiceProvider.GetService<ILedgerScribbler>()?.YieldToOuterActivity(ledgerScribbler);

        // do stuff
        var _service = _scope.ServiceProvider.GetService<IService>()
            ?? throw new InvalidOperationException(@"service type not registered");
        scopeActions?.Invoke(_service);
    }
}
