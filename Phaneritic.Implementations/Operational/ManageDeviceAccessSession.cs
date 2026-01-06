using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Operational;
using System.Collections.Concurrent;

namespace Phaneritic.Implementations.Operational;
public class ManageDeviceAccessSession(
    IOperationalContext operationalContext,
    IWorkCommitter workCommitter,
    IAccessorReader accessorReader,
    IAccessSessionReader accessSessionReader,
    IAccessMechanismReader accessMechanismReader,
    ICanonicalDictionary<AccessMechanismKey, AccessSessionDto> deviceSessions,
    ICanonicalDictionary<AccessMechanismID, ConcurrentDictionary<MethodKey, OperationDto>> deviceOperations
    ) : ManageAccessSessionBase(operationalContext, accessorReader, accessSessionReader, workCommitter), IManageAccessSession
{
    public int Priority => 10;

    /// <summary>does not pool</summary>
    public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismTypeKey accessMechanismTypeKey)
        => null;

    public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismKey accessMechanismKey)
    {
        var _mechanism = accessMechanismReader.GetAccessMechanism(accessMechanismKey)
           ?? throw new ArgumentException($@"mechanism [{accessMechanismKey}] not found", nameof(accessMechanismKey));
        if (!_mechanism.AccessMechanismType.IsUserAccess)
        {
            var _dto = StartAccessSession(accessorID, _mechanism);
            if (_dto != null)
            {
                // add to caches
                deviceSessions.AddOrUpdate(accessMechanismKey, _dto, (_, _) => _dto);
                deviceOperations.AddOrUpdate(_mechanism.AccessMechanismID, [], (_, _) => []);
                return _dto;
            }
        }
        return null;
    }

    public bool StopAccessSession(AccessSessionID accessSessionID)
    {
        var _session = AccessSessionReader.GetAccessSession(accessSessionID);
        if (_session != null)
        {
            if (!(_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? true))
            {
                var _now = DateTimeOffset.Now;
                var _ops = GetOperations(_session.AccessSessionID);

                // ignore transients
                TerminateRemainingOperations(_ops, _now);
                TerminateAccessSession(accessSessionID, _now);

                // blast from caches
                deviceSessions.TryRemove(_session.AccessMechanism?.AccessMechanismKey ?? default);
                deviceOperations.TryRemove(_session.AccessMechanism?.AccessMechanismID ?? default);

                // TODO: rate tracking
                WorkCommitter.CommitWork(OperationalContext);
                return true;
            }
        }
        return false;
    }
}
