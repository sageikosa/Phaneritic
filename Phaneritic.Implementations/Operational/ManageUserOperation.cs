using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Operational;

public class ManageUserOperation(
    IOperationalContext operationalContext,
    IWorkCommitter workCommitter,
    ILudDictionary<MethodKey, MethodDto> methods,
    IAccessSessionReader accessSessionReader,
    IEnumerable<IProvideScopedOperations> provideOperations
    ) : IManageOperation
{
    private Lis t<IProvideScopedOperations> ProvideOperations = [.. provideOperations];

    public int Priority => 100;

    public OperationDto? StartOperation(MethodKey methodKey)
    {
        if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
            && (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false))
        {
            // not for user access implies device-only
            if (methods.Get(methodKey) is MethodDto _method)
            {
                // make new op
                var _now = DateTimeOffset.Now;
                var _newOp = new Operation
                {
                    AccessMechanismID = _session.AccessMechanism.AccessMechanismID,
                    MethodKey = methodKey,
                    AccessorID = _session.Accessor?.AccessorID ?? default,
                    AccessSessionID = _session.AccessSessionID,
                    StartedAt = _now
                };
                operationalContext.Operations.Add(_newOp);
                operationalContext.OperationLogs.Add(
                    new OperationLog()
                    {
                        AccessMechanismID = _newOp.AccessMechanismID,
                        MethodKey = methodKey,
                        AccessorID = _newOp.AccessorID,
                        AccessSessionID = _newOp.AccessSessionID,
                        LogTime = _now,
                        OperationID = _newOp.OperationID,
                    });
                workCommitter.CommitWork(operationalContext);
                ProvideOperations.ClearCache();

                // return and canonical dictionary
                var _opDto = new OperationDto
                {
                    AccessMechanismID = _newOp.AccessMechanismID,
                    AccessorID = _newOp.AccessorID,
                    MethodKey = methodKey,
                    OperationID = _newOp.OperationID,
                    StartedAt = _now
                };
                return _opDto;
            }
            else
            {
                throw new InvalidOperationException($@"method [{methodKey}] not defined ");
            }
        }
        return null;
    }

    public bool StopOperation(OperationID operationID)
    {
        if ((accessSessionReader.GetScopedAccessSession() is AccessSessionDto _session)
            && (_session.AccessMechanism?.AccessMechanismType.IsUserAccess ?? false))
        {
            var _current = operationalContext.Operations
                .Where(_o => _o.OperationID == operationID)
                .FirstOrDefault();
            if (_current != null)
            {
                operationalContext.Operations.Remove(_current);
                operationalContext.OperationLogs.Add(
                    new OperationLog()
                    {
                        AccessSessionID = _session.AccessSessionID,
                        OperationID = operationID,
                        AccessMechanismID = _current.AccessMechanismID,
                        AccessorID = _current.AccessorID,
                        IsComplete = true,
                        LogTime = DateTime.UtcNow,
                        MethodKey = _current.MethodKey
                    });
                workCommitter.CommitWork(operationalContext);
                ProvideOperations.ClearCache();
            }
            return true;
        }
        return false;
    }
}
