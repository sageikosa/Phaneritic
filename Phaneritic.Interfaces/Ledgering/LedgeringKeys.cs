using Phaneritic.Generators.Attributes;

namespace Phaneritic.Interfaces.Ledgering;

public readonly partial struct ActivityTypeKey
{
    public static readonly ActivityTypeKey Undefined = new(nameof(Undefined));
}
public readonly partial struct ActivityCategory;

public readonly partial struct InfoEntryKey;
public readonly partial struct InfoEntryValue;

public readonly partial struct DemandEntryLine;
public readonly partial struct FulfillmentEntryLine;

[StrongKeys]
public static class LedgeringKeys
{
    public const int ActivityTypeKey = 16;
    public const int ActivityCategory = 16;
    public const int InfoEntryKey = 16;
    public const int InventoryEntryLine = 8;
    public const int DemandEntryLine = 8;
    public const int FulfillmentEntryLine = 8;
}

[StrongKeys(IsUnicodeStorage = true)]
public static class LedgeringUnicodeKeys
{
    public const int InfoEntryValue = 256;
}
