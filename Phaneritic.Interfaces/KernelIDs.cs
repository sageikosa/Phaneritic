using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces;

// NOTE: defined here as partial to make renaming easier in Visual Studio

public readonly partial record struct TestID;
public readonly partial record struct Test2ID;

[StrongIDs]
public enum KernelIDs
{
    TestID,
    Test2ID
}
