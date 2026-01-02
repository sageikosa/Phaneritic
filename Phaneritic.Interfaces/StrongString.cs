namespace Phaneritic.Interfaces;

/// <summary>Strongly typed string based key source</summary>
public readonly struct StrongString(string keyVal) : IEquatable<StrongString>
{
    private readonly string _KeyVal = keyVal;

    /// <summary>Default = string.Empty.  Allow initialization.</summary>
    public string KeyVal { readonly get => _KeyVal ?? string.Empty; init => _KeyVal = value; }

    public bool Equals(StrongString other)
        => string.Equals(KeyVal, other.KeyVal, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode()
        => StringComparer.OrdinalIgnoreCase.GetHashCode(KeyVal);

    public static implicit operator string(StrongString strong)
        => strong.KeyVal;

    public static implicit operator StrongString(string strong)
        => new(strong);

    public override bool Equals(object? obj)
        => obj is StrongString _val && Equals(_val);

    public static bool operator ==(StrongString left, StrongString right)
        => left.Equals(right);

    public static bool operator !=(StrongString left, StrongString right)
        => !(left == right);

    public override string ToString()
        => $@"{{ {nameof(KeyVal)} = ""{KeyVal}"" }}";
}
