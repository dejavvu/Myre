﻿using System;
using System.Runtime.InteropServices;

namespace Myre
{
    /// <summary>
    /// Provides a safe way of bitwise converting a Single to a Uint32
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct SingleUIntUnion
    {
        /// <summary>
        /// The value of this union, interpreted as a single
        /// </summary>
        [FieldOffset(0)]
        public float SingleValue;

        /// <summary>
        /// The value of this union, interpreted as a uint
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public uint UIntValue;
    }

    /// <summary>
    /// Provides a safe way of bitwise converting a Double to a ULong
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct DoubleULongUnion
    {
        /// <summary>
        /// The value of this union, interpreted as a double
        /// </summary>
        [FieldOffset(0)]
        public double DoubleValue;

        /// <summary>
        /// The value of this union, interpreted as a ulong
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public ulong ULongValue;
    }

    /// <summary>
    /// Provides a safe way of bitwise converting a Long to a ULong
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct LongULongUnion
    {
        /// <summary>
        /// The value of this union, interpreted as a long
        /// </summary>
        [FieldOffset(0)]
        public long LongValue;

        /// <summary>
        /// The value of this union, interpreted as a ulong
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public ulong ULongValue;
    }

    /// <summary>
    /// Provides a safe way of converting an Int32 to a UInt32
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct IntUIntUnion
    {
        /// <summary>
        /// The value of this union, interpreted as a int
        /// </summary>
        [FieldOffset(0)]
        public int IntValue;

        /// <summary>
        /// The value of this union, interpreted as a uint
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public uint UIntValue;
    }
}
