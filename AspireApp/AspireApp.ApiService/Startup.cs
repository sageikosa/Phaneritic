using Microsoft.Extensions.DependencyInjection.Extensions;
using Phaneritic.Implementations;
using Phaneritic.Implementations.Commands.Ledgering;
using Phaneritic.Implementations.Commands.Operational;
using Phaneritic.Implementations.CommitWork;
using Phaneritic.Implementations.Database;
using Phaneritic.Implementations.DtoPack.Ledgering;
using Phaneritic.Implementations.DtoPack.Operational;
using Phaneritic.Implementations.EF.TableCache;
using Phaneritic.Implementations.LudCache;
using Phaneritic.Implementations.Models.Ledgering;
using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Implementations.Operational;
using Phaneritic.Implementations.Queries.Operational;
using Phaneritic.Implementations.Sempahores;
using Phaneritic.Implementations.Startup;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Database;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;
using System.Net.NetworkInformation;

namespace AspireApp.Web;

public class Startup(
    IConfiguration configuration
    )
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.Configure<DatabaseOptions>(_o => Configuration.GetSection("DatabaseOptions").Bind(_o));

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
        services.AddOperationalIDsConfigurators();
        services.AddOperationalLongIDsConfigurators();
        services.AddLedgeringIDsConfigurators();
        services.AddOperationalKeysConfigurators();
        services.AddOperationalUnicodeKeysConfigurators();
        services.AddLedgeringKeysConfigurators();
        services.AddLedgeringUnicodeKeysConfigurators();

        services.AddHostedService<LudCacheFreshnessPoller>();
        services.AddImplementationsRefreshers();

        // operational implementations
        services.AddScoped<IOperationalContext, OperationalContext>();
        services.AddSingleton<IPackRecord<ProcessNode, ProcessNodeDto>, ProcessNodeDtoPack>();
        services.AddSingleton<IPackRecord<ProcessNodeType, ProcessNodeTypeDto>, ProcessNodeTypeDtoPack>();
        services.AddSingleton<IPackRecord<OptionGroup, OptionGroupDto>, OptionGroupDtoPack>();
        services.AddSingleton<IPackRecord<OptionType, OptionTypeDto>, OptionTypeDtoPack>();
        services.AddSingleton<IPackRecord<Option, OptionDto>, OptionDtoPack>();

        services.AddSingleton<IPackRecord<Method, MethodDto>, MethodDtoPack>();
        services.AddSingleton<IPackRecord<AccessGroup, AccessGroupDto>, AccessGroupDtoPack>();
        services.AddSingleton<IPackRecord<Accessor, AccessorDto>, AccessorDtoPack>();
        services.AddSingleton<IPackRecord<AccessorCredential, AccessorCredentialDto>, AccessorCredentialDtoPack>();
        services.AddSingleton<IPackRecord<AccessorCredentialType, AccessorCredentialTypeDto>, AccessorCredentialTypeDtoPack>();
        services.AddSingleton<IPackRecord<AccessorType, AccessorTypeDto>, AccessorTypeDtoPack>();
        services.AddSingleton<IPackRecord<AccessMechanism, AccessMechanismDto>, AccessMechanismDtoPack>();
        services.AddSingleton<IPackRecord<AccessMechanismType, AccessMechanismTypeDto>, AccessMechanismTypeDtoPack>();

        services.AddScoped<IAccessorReader, AccessorReader>();
        services.AddScoped<IAccessMechanismReader, AccessMechanismReader>();
        services.AddScoped<IAccessSessionReader, AccessSessionReader>();
        services.AddScoped<IOperationReader, OperationReader>();

        services.AddSingleton<IProcessNodesNavigator, ProcessNodesNavigator>();
        services.AddSingleton<IOptionsNavigator, OptionsNavigator>();
        services.AddScoped<ILudCacheFreshnessNotify, OptionCacheRefresher>();

        services.AddScoped<IExplicitActivity, ExplicitActivity>();
        services.AddScoped<IProvideAccessMechanism, ProvideAccessMechanism>();
        services.AddScoped<IProvideAccessMechanism, ProvideExplicitAccessMechanism>();

        services.AddScoped<IProvideAccessSession, ProvideExplicitAccessSession>();
        services.AddScoped<IProvideAccessSession, ProvideCachedAccessSession>();
        services.AddScoped<IProvideAccessSession, ProvideUserAccessSession>();
        services.AddScoped<IProvideScopedOperations, ProvideScopedDeviceOperations>();
        services.AddScoped<IProvideScopedOperations, ProvideScopedUserOperations>();
        services.AddScoped<IManageAccessSession, ManageUserAccessSession>();
        services.AddScoped<IManageAccessSession, ManageDeviceAccessSession>();
        services.AddScoped<IManageOperation, ManageUserOperation>();
        services.AddScoped<IManageOperation, ManageDeviceOperation>();
        services.AddScoped<IScopeIsolateActivity, ScopeIsolateActivity>();

        // ledgering implementations
        services.AddScoped<ILedgeringContext, LedgeringContext>();
        services.AddSingleton<IPackRecord<Activity, ActivityDto>, ActivityDtoPack>();
        services.AddSingleton<IPackRecord<ActivityType, ActivityTypeDto>, ActivityTypeDtoPack>();
        services.AddSingleton<IPackRecord<InfoEntry, InfoEntryDto>, InfoEntryDtoPack>();
        services.AddSingleton<IPackRecord<ExceptionEntry, ExceptionEntryDto>, ExceptionEntryDtoPack>();
        services.AddScoped<ILedgerScribbler, LedgerScribbler>();
    }
}
