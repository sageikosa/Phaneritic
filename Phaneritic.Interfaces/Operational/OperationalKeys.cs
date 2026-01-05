using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces.Operational;

public readonly partial struct ProcessNodeTypeKey;
public readonly partial struct ProcessNodeKey;
public readonly partial struct OperationalStateTypeKey;
public readonly partial struct OperationalStateKey;
public readonly partial struct OptionTypeKey;
public readonly partial struct OptionGroupKey;
public readonly partial struct AccessMechanismKey;
public readonly partial struct AccessMechanismTypeKey;
public readonly partial struct AccessorKey;
public readonly partial struct AccessorTypeKey;
public readonly partial struct AccessGroupKey;
public readonly partial struct MethodKey;
public readonly partial struct TaskMutexKey;
public readonly partial struct TaskKey;

[StrongKeys]
public static class OperationalKeys
{
    public const int ProcessNodeTypeKey = 16;
    public const int ProcessNodeKey = 32;
    public const int OperationalStateTypeKey = 16;
    public const int OperationalStateKey = 16;
    public const int OptionTypeKey = 32;
    public const int OptionGroupKey = 16;
    public const int AccessMechanismKey = 64;
    public const int AccessMechanismTypeKey = 16;
    public const int AccessorKey = 64;
    public const int AccessorCredentialTypeKey = 8;
    public const int AccessorTypeKey = 16;
    public const int AccessGroupKey = 32;
    public const int MethodKey = 16;
    public const int TaskMutexKey = 8;
    public const int TaskKey = 24;
}

public readonly partial struct OptionValue;
public readonly partial struct CredentialValue;

[StrongKeys(IsUnicodeStorage = true, IsCaseSensitive = true)]
public static class OperationalUnicodeKeys
{
    public const int OptionValue = 256;
    public const int CredentialValue = 512;
}
