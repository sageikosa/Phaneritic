// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Phaneritic.Interfaces;

/// <devdoc>
///    <para>Provides a simple light bit vector with easy integer or Boolean access to
///       a 64 bit storage.</para>
/// </devdoc>
public struct BitVector64
{
    private ulong _data;

    /// <devdoc>
    /// <para>Initializes a new instance of the BitVector64 structure with the specified internal data.</para>
    /// </devdoc>
    public BitVector64(ulong data)
    {
        _data = data;
    }

    /// <devdoc>
    /// <para>Initializes a new instance of the BitVector64 structure with the information in the specified
    ///    value.</para>
    /// </devdoc>
    public BitVector64(BitVector64 value)
    {
        _data = value._data;
    }

    /// <devdoc>
    ///    <para>Gets or sets a value indicating whether all the specified bits are set.</para>
    /// </devdoc>
    public bool this[ulong bit]
    {
        readonly get => (_data & bit) == bit;
        set
        {
            if (value)
            {
                _data |= bit;
            }
            else
            {
                _data &= ~bit;
            }
        }
    }

    /// <devdoc>
    ///    <para>Gets or sets the value for the specified section.</para>
    /// </devdoc>
    public ulong this[Section section]
    {
        readonly get
        {
            unchecked
            {
                return (_data & (ulong)section.Mask << section.Offset) >> section.Offset;
            }
        }
        set
        {
            // The code should really have originally validated "(value & section.Mask) == value" with
            // an exception (it instead validated it with a Debug.Assert, which does little good in a
            // public method when in a Release build).  We don't include such a check now as it would
            // likely break things and for little benefit.

            value <<= section.Offset;
            var offsetMask = (0xFFFFFFFF & (ulong)section.Mask) << section.Offset;
            _data = unchecked(_data & ~offsetMask | value & offsetMask);
        }
    }

    /// <devdoc>
    ///    returns the raw data stored in this bit vector...
    /// </devdoc>
    public readonly ulong Data => _data;

    /// <devdoc>
    ///    <para> Creates the first mask in a series.</para>
    /// </devdoc>
    public static long CreateMask()
        => CreateMask(0);

    /// <devdoc>
    ///     Creates the next mask in a series.
    /// </devdoc>
    public static long CreateMask(long previous)
    {
        if (previous == 0)
            return 1;

        if (previous == unchecked((long)0x8000_0000_0000_0000))
            throw new InvalidOperationException(@"Bit Vector Full");

        return previous << 1;
    }

    /// <devdoc>
    ///    <para>Creates the first section in a series, with the specified maximum value.</para>
    /// </devdoc>
    public static Section CreateSection(uint maxValue)
        => CreateSectionHelper(maxValue, 0, 0);

    /// <devdoc>
    ///    <para>Creates the next section in a series, with the specified maximum value.</para>
    /// </devdoc>
    public static Section CreateSection(uint maxValue, Section previous)
        => CreateSectionHelper(maxValue, previous.Mask, previous.Offset);

    private static Section CreateSectionHelper(uint maxValue, uint priorMask, short priorOffset)
    {
        if (maxValue < 1)
            throw new ArgumentException(@"max value too small", nameof(maxValue));

        var offset = (short)(priorOffset + BitOperations.PopCount(priorMask));
        if (offset >= 64)
            throw new InvalidOperationException(@"Bit Vector Full");

        var mask = BitOperations.RoundUpToPowerOf2(maxValue + 1) - 1;
        return new Section(mask, offset);
    }

    public override readonly bool Equals([NotNullWhen(true)] object? o)
        => o is BitVector64 other && _data == other._data;

    public override readonly int GetHashCode()
        => _data.GetHashCode();

    public static string ToString(BitVector64 value)
    {
        return string.Create(/*"BitVector64{".Length*/12 + /*64 bits*/64 + /*"}".Length"*/1, value, (dst, v) => {
            ReadOnlySpan<char> prefix = "BitVector64{";
            prefix.CopyTo(dst);
            dst[^1] = '}';

            var locdata = v._data;
            dst = dst.Slice(prefix.Length, 64);
            for (var i = 0; i < dst.Length; i++)
            {
                dst[i] = (locdata & 0x8000_0000_0000_0000) != 0 ? '1' : '0';
                locdata <<= 1;
            }
        });
    }

    public override readonly string ToString()
        => ToString(this);

    public static bool operator ==(BitVector64 left, BitVector64 right) 
        => left.Equals(right);

    public static bool operator !=(BitVector64 left, BitVector64 right) 
        => !(left == right);

    /// <devdoc>
    ///    <para>
    ///       Represents an section of the vector that can contain a integer number.</para>
    /// </devdoc>
    public readonly struct Section
    {
        private readonly uint _mask;
        private readonly short _offset;

        internal Section(uint mask, short offset)
        {
            _mask = mask;
            _offset = offset;
        }

        public uint Mask => _mask;

        public short Offset => _offset;

        public override bool Equals([NotNullWhen(true)] object? o) => o is Section other && Equals(other);

        public bool Equals(Section obj)
        {
            return obj._mask == _mask && obj._offset == _offset;
        }

        public static bool operator ==(Section a, Section b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Section a, Section b)
        {
            return !(a == b);
        }

        public override int GetHashCode() => HashCode.Combine(_mask, _offset);

        public static string ToString(Section value)
        {
            return $"Section{{0x{value.Mask:x}, 0x{value.Offset:x}}}";
        }

        public override string ToString()
        {
            return ToString(this);
        }
    }
}
