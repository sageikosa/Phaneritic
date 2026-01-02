using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Phaneritic.Implementations.CommitWork;
using Phaneritic.Implementations.Database;
using Phaneritic.Implementations.EF.TableCache;
using Phaneritic.Implementations.LudCache;
using Phaneritic.Implementations.Sempahores;
using Phaneritic.Implementations.Startup;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Database;
using Phaneritic.Interfaces.LudCache;

namespace Phaneritic.Implementations;

public static class KernelDependencies
{
    // TODO: configure options...
    public static IServiceCollection ConfigureKernel(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GyroDatabaseOptions>(_o => configuration.GetSection(@"kernelDbOptions").Bind(_o));
        return services;
    }

    /// <summary>register core DI services</summary>
    public static IServiceCollection AddPhaneriticKernel(this IServiceCollection services)
    {
        services.TryAddScoped<IWorkCommitter, WorkCommitter>();

        services.TryAddScoped<IDbErrorWrap, SqlErrorRetry>();
        services.TryAddScoped<IDbScopedCommands, DbScopedCommands>();
        services.TryAddTransient<IDbTransientCommands, DbScopedCommands>();
        services.TryAddScoped<IDbLoggingCommands, DbLoggingCommands>();
        services.TryAddTransient<IDbTransientLoggingCommands, DbLoggingCommands>();
        services.AddScoped<IDbScopedConnection, DbScopedConnection>();
        services.AddScoped<IDbLoggingConnection, DbLoggingConnection>();

        services.TryAddSingleton(typeof(ICanonicalDictionary<,>), typeof(CanonicalDictionary<,>));
        services.AddTransient<IScopeAction, ScopeAction>();

        services.TryAddSingleton<ILudCacheFreshness, LudCacheFreshness>();
        services.TryAddSingleton(typeof(ILudDictionary<,>), typeof(BaseLudDictionary<,>));
        services.TryAddScoped<ITableFreshnessContext, TableFreshnessContext>();
        services.TryAddTransient<ITableFreshnessContextTransient, TableFreshnessContext>();
        services.TryAddScoped<ILudCacheRefreshAll, LudCacheRefreshAll>();
        services.TryAddScoped<IKickStart, LudCacheKickStart>();
        services.TryAddScoped(typeof(ILudCacheUpdate<>), typeof(LudCacheUpdate<>));
        services.TryAddScoped(typeof(ILudCacheGetFreshness<>), typeof(LudCacheGetFreshness<>));

        services.AddSingleton(typeof(IIndexedCriticalSectionDispenser<,>), typeof(IndexedCriticalSectionDispenser<,>));

        // SemaphoreBarriers used here should be registered "global" in DI container
        services.TryAddScoped(typeof(CriticalSection<>));

        // SemaphoreBarriers used here tracked in ICanonicalDictionary<,> (a singleton)
        services.TryAddScoped(typeof(IndexedCriticalSection<,>));

        // Ef property configurators
        services.AddCommonKeysConfigurators();
        services.AddKernelIDsConfigurators();
        services.AddKernelKeysConfigurators();

        services.AddHostedService<LudCacheFreshnessPoller>();

        return services;
    }
}
