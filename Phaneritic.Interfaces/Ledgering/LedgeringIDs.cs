using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces.Ledgering;

public readonly partial record struct ActivityID;

[StrongIDs]
public enum LedgeringIDs : long
{
    ActivityID
}
