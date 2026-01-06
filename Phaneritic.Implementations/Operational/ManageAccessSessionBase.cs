using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

public class ManageAccessSessionBase(
    IOperationalContext operationalContext,
    IAccessorReader accessorReader,
    IAccessSessionReader accessSessionReader,
    IWorkCommitter workCommitter
    )
{
    protected readonly IOperationalContext OperationalContext = operationalContext;
    protected readonly IAccessorReader AccessorReader = accessorReader;
    protected readonly IAccessSessionReader AccessSessionReader = accessSessionReader;
    protected readonly IWorkCommitter WorkCommitter = workCommitter;

    protected List<Operation> GetOperations(AccessSessionID sessionID)
        => [.. OperationalContext.Operations
            .Include(_o => _o.Method)
            .Where(_o => _o.AccessSessionID == sessionID)];

    protected void TerminateAccessSession(AccessSessionID accessSessionID, DateTimeOffset now)
    {
        var _old = OperationalContext.AccessSessions.Find(accessSessionID);
        if (_old != null)
        {
            OperationalContext.AccessSessions.Remove(_old);
            OperationalContext.OperationLogs.Add(new()
            {
                AccessSessionID = _old.AccessSessionID,
                LogTime = now,
                AccessMechanismID = _old.AccessMechanismID,
                AccessorID = _old.AccessorID,
                IsComplete = true
            });
        }
    }

    protected void TerminateRemainingOperations(List<Operation> operations, DateTimeOffset now)
    {
        foreach (var _op in operations)
        {
            // remove operations
            OperationalContext.Operations.Remove(_op);
            OperationalContext.OperationLogs.Add(new()
            {
                AccessSessionID = _op.AccessSessionID,
                OperationID = _op.OperationID,
                MethodKey = _op.MethodKey,
                LogTime = now,
                AccessMechanismID = _op.AccessMechanismID,
                AccessorID = _op.AccessorID,
                IsComplete = true
            });
        }
    }

    protected void TerminateTransientOperations(List<Operation> operations, DateTimeOffset now)
    {
        foreach (var _op in operations.Where(_op => _op.Method!.IsTransient).ToList())
        {
            // remove transient operations
            OperationalContext.Operations.Remove(_op);
            OperationalContext.OperationLogs.Add(new()
            {
                AccessSessionID = _op.AccessSessionID,
                OperationID = _op.OperationID,
                MethodKey = _op.MethodKey,
                LogTime = now,
                AccessMechanismID = _op.AccessMechanismID,
                AccessorID = _op.AccessorID,
                IsComplete = true
            });
        }
    }

    /// <summary>transfers non-transient operations to target AccessSession</summary>
    protected void TransferPersistentOperations(List<Operation> source, AccessSession target, DateTimeOffset now)
    {
        foreach (var _op in source.Where(_o => !_o.Method!.IsTransient).ToList())
        {
            // transfer all moveable stuff
            _op.AccessSessionID = target.AccessSessionID;
            _op.AccessorID = target.AccessorID;
            _op.AccessMechanismID = target.AccessMechanismID;

            OperationalContext.OperationLogs.Add(new()
            {
                AccessSessionID = target.AccessSessionID,
                OperationID = _op.OperationID,
                MethodKey = _op.MethodKey,
                LogTime = now,
                AccessMechanismID = _op.AccessMechanismID,
                AccessorID = _op.AccessorID
            });

            // remove from source
            source.Remove(_op);
        }
    }
    protected AccessSessionDto? StartAccessSession(AccessorID accessorID, AccessMechanismDto accessMechanism)
    {
        var _now = DateTimeOffset.Now;
        var _accessor = AccessorReader.GetAccessor(accessorID)
            ?? throw new ArgumentException($@"accessor [{accessorID}] not found", nameof(accessorID));

        // start session
        var _startingSession = new AccessSession
        {
            StartedAt = _now,
            AccessorID = accessorID,
            AccessMechanismID = accessMechanism.AccessMechanismID
        };
        OperationalContext.AccessSessions.Add(_startingSession);
        OperationalContext.OperationLogs.Add(new()
        {
            AccessSessionID = _startingSession.AccessSessionID,
            LogTime = _now,
            AccessMechanismID = accessMechanism.AccessMechanismID,
            AccessorID = accessorID
        });

        if (accessMechanism.Location != null)
        {
            // location bound device, methods that stay over sessions need to move
            var _oldMechSession = AccessSessionReader.GetAccessSession(accessMechanism.AccessMechanismID);
            if (_oldMechSession != null)
            {
                var _ops = GetOperations(_oldMechSession.AccessSessionID);
                TransferPersistentOperations(_ops, _startingSession, _now);
                TerminateRemainingOperations(_ops, _now);
                TerminateAccessSession(_oldMechSession.AccessSessionID, _now);
            }
        }
        else
        {
            // not location bound
            var _oldAccessorSession = AccessSessionReader.GetAccessSessions(accessorID)
                .FirstOrDefault(_s => _s.AccessMechanism?.AccessMechanismType.AccessMechanismTypeKey == accessMechanism.AccessMechanismType.AccessMechanismTypeKey);

            // accessor may maintain active sessions on different device types
            if (_oldAccessorSession != null)
            {
                var _ops = GetOperations(_oldAccessorSession.AccessSessionID);
                TransferPersistentOperations(_ops, _startingSession, _now);
                TerminateRemainingOperations(_ops, _now);
                TerminateAccessSession(_oldAccessorSession.AccessSessionID, _now);
            }
        }

        // TODO: rate tracking
        WorkCommitter.CommitWork(OperationalContext);

        // started
        return new AccessSessionDto
        {
            AccessSessionID = _startingSession.AccessSessionID,
            AccessMechanism = accessMechanism,
            Accessor = _accessor,
            StartedAt = _now
        };
    }
}