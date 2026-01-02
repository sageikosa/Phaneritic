using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces;

public readonly partial struct NameString;
public readonly partial struct DescriptionString;

[StrongKeys(IsUnicodeStorage = true)]
public static class CommonKeys
{
    public const int NameString = 64;
    public const int DescriptionString = 256;
}
