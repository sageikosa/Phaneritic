using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Implementations.Sempahores;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Commands.Operational;
public class ManageUserAccessSession(
    IOperationalContext operationalContext,
    IWorkCommitter workCommitter,
    IAccessMechanismReader accessMechanismReader,
    IAccessorReader accessorReader,
    IAccessSessionReader accessSessionReader,
    ILudDictionary<AccessMechanismTypeKey, AccessMechanismTypeDto> accessMechanismTypes,
    IndexedCriticalSection<AccessMechanismTypeKey, AccessMechanismTypeBarrier> poolBarrier
    ) : ManageAccessSessionBase(operationalContext, accessorReader, accessSessionReader, workCommitter), IManageAccessSession
{
    public int Priority => 100;

    public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismKey accessMechanismKey)
    {
        var _mechanism = accessMechanismReader.GetAccessMechanism(accessMechanismKey)
            ?? throw new ArgumentException($@"mechanism [{accessMechanismKey}] not found", nameof(accessMechanismKey));
        if (!_mechanism.AccessMechanismType.IsPoolable && _mechanism.AccessMechanismType.IsUserAccess)
        {
            return StartAccessSession(accessorID, _mechanism);
        }
        return null;
    }

    public AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismTypeKey accessMechanismTypeKey)
    {
        if (accessMechanismTypes.Get(accessMechanismTypeKey) is not AccessMechanismTypeDto _mechType)
        {
            throw new ArgumentException($@"accessMechanismType [{accessMechanismTypeKey}] not found", nameof(accessMechanismTypeKey));
        }
        if (!_mechType.IsUserAccess)
        {
            // fail fast
            return null;
        }

        var _accessor = AccessorReader.GetAccessor(accessorID)
            ?? throw new ArgumentException($@"accessor [{accessorID}] not found", nameof(accessorID));

        // try to use mechanism of same type if already in use by the accessor
        var _exact = OperationalContext.AccessSessions
            .FirstOrDefault(_am => _am.AccessorID == accessorID
                && _am.AccessMechanism!.AccessMechanismTypeKey == accessMechanismTypeKey);
        var _targetMechanismID = _exact?.AccessMechanismID ?? default;
        if (_targetMechanismID == default)
        {
            // didn't have an access mechanism of that type, so find new one
            if (_mechType.IsPoolable)
            {
                // TODO: configure timeout
                if (poolBarrier.TryEnter(accessMechanismTypeKey, 3000))
                {
                    // find an enabled and non-sessioning mechanismID of the correct type
                    _targetMechanismID = OperationalContext.AccessMechanisms
                        .Where(_am => _am.AccessMechanismTypeKey == accessMechanismTypeKey
                        && _am.IsEnabled
                        && !OperationalContext.AccessSessions.Any(_as => _as.AccessMechanismID == _am.AccessMechanismID))
                        .Select(_am => _am.AccessMechanismID)
                        .FirstOrDefault();
                }
            }
            else
            {
                throw new InvalidOperationException($@"accessMechanismType [{accessMechanismTypeKey}] invalid for user access via pooling");
            }
        }

        if (_targetMechanismID != default)
        {
            // should be found...but protect anyway
            var _mechanism = accessMechanismReader.GetAccessMechanism(_targetMechanismID)
               ?? throw new InvalidOperationException($@"mechanism [{_targetMechanismID}] not found");

            // can just use the other method now
            return StartAccessSession(accessorID, _mechanism);
        }
        return null;
    }

    public bool StopAccessSession(AccessSessionID accessSessionID)
    {
        var _session = AccessSessionReader.GetAccessSession(accessSessionID);
        if (_session != null)
        {
            if (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false)
            {
                var _now = DateTimeOffset.Now;
                var _ops = GetOperations(accessSessionID);
                TerminateTransientOperations(_ops, _now);
                if (_ops.Count == 0)
                {
                    TerminateAccessSession(accessSessionID, _now);
                }
                else
                {
                    // non-transient operations need to stay with a session
                    // still need to log the session "stopped"
                    OperationalContext.OperationLogs.Add(new()
                    {
                        AccessSessionID = _session.AccessSessionID,
                        LogTime = _now,
                        AccessMechanismID = _session.AccessMechanism?.AccessMechanismID ?? default,
                        AccessorID = _session.Accessor?.AccessorID ?? default,
                        IsComplete = true
                    });
                }

                // TODO: rate tracking
                WorkCommitter.CommitWork(OperationalContext);
                return true;
            }
        }
        return false;
    }
}
