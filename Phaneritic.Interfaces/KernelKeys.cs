using GyroLedger.Generators.Attributes;

namespace GyroLedger.CodeInterface;

// NOTE: defined here as partial to make renaming easier in Visual Studio

public readonly partial struct RefresherKey;

[StrongKeys]
public static class KernelKeys
{
    public const int RefresherKey = 64;
}
