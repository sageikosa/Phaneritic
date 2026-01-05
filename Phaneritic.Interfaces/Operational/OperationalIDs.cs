using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces.Operational;

public readonly partial record struct AccessorID;
public readonly partial record struct AccessMechanismID;

public readonly partial record struct OperationID;
public readonly partial record struct OperationLogID;
public readonly partial record struct AccessSessionID;

[StrongIDs]
public enum OperationalIDs
{
    AccessorID,
    AccessMechanismID
}

[StrongIDs]
public enum OperationalLongIDs : long
{
    OperationID,
    OperationLogID,
    AccessSessionID
}