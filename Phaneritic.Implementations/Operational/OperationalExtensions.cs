using Phaneritic.Interfaces.Operational;
using Phaneritic.Implementations.Commands.Operational;
using Phaneritic.Implementations.Operational;
using System.Collections.Frozen;

namespace Phaneritic.Implementations.Operational;

public static class OperationalExtensions
{
    extension(List<IProvideScopedOperations> providers)
    {
        public void ClearCache()
        {
            foreach (var _p in providers)
            {
                _p.ClearCache();
            }
        }

        public FrozenSet<MethodKey> GetScopedMethods()
            => providers.OrderBy(_p => _p.Priority)
            .Select(_p => _p.CurrentMethods)
            .OfType<FrozenSet<MethodKey>>()
            .FirstOrDefault() ?? [];

        public FrozenSet<OperationDto> GetScopedOperations()
            => providers.OrderBy(_p => _p.Priority)
            .Select(_p => _p.CurrentOperations)
            .OfType<FrozenSet<OperationDto>>()
            .FirstOrDefault() ?? [];
    }

    extension(List<IManageAccessSession> managers)
    {
        public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismKey accessMechanismKey)
            => (from _m in managers
                orderby _m.Priority
                let _as = _m.StartAccessSession(accessorID, accessMechanismKey)
                where _as != null
                select _as)
            .FirstOrDefault();

        public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismTypeKey accessMechanismTypeKey)
            => (from _m in managers
                orderby _m.Priority
                let _as = _m.StartAccessSession(accessorID, accessMechanismTypeKey)
                where _as != null
                select _as)
            .FirstOrDefault();

        public bool StopAccessSession(AccessSessionID accessSessionID)
            => (from _m in managers
                orderby _m.Priority
                let _as = _m.StopAccessSession(accessSessionID)
                where _as
                select _as)
            .FirstOrDefault();
    }

    public static AccessMechanismDto? GetScopedAccessMechanism(this IEnumerable<IProvideAccessMechanism> providers)
        => providers.OrderBy(_p => _p.Priority)
        .Select(_p => _p.CurrentAccessMechanism)
        .OfType<AccessMechanismDto>()
        .FirstOrDefault();

    public static AccessSessionDto? GetScopedAccessSession(this IEnumerable<IProvideAccessSession> providers)
        => providers.OrderBy(_p => _p.Priority)
        .Select(_p => _p.CurrentAccessSession)
        .OfType<AccessSessionDto>()
        .FirstOrDefault();

    public static AccessorDto? GetScopedAccessor(this IEnumerable<IProvideAccessor> providers)
        => providers.OrderBy(_p => _p.Priority)
        .Select(_p => _p.CurrentAccessor)
        .OfType<AccessorDto>()
        .FirstOrDefault();

    public static OperationDto? StartOperation(this List<IManageOperation> operationManagers, MethodKey methodKey)
        => (from _om in operationManagers
            orderby _om.Priority
            let _op = _om.StartOperation(methodKey)
            where _op != null
            select _op)
        .FirstOrDefault();
}
