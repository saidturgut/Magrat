namespace Mos6502.Computing;
using Kernel.Devices;

// FLAGS REGISTER UNIT
public class Fru : IFru
{
    public bool Check(byte condition, Flags tmp) => condition switch
    {
        0 => !tmp.Bit1,
        1 => tmp.Bit1,
        2 => !tmp.Bit0,
        3 => tmp.Bit0,
        4 => !tmp.Bit6,
        5 => tmp.Bit6,
        6 => !tmp.Bit7,
        7 => tmp.Bit7,
        _ => true
    };
}

[Flags]
public enum Flag
{
    NONE = 0,
    CARRY = 1 << 0,
    ZERO = 1 << 1,
    INTERRUPT = 1 << 2,
    DECIMAL = 1 << 3,
    BREAK = 1 << 4,
    UNUSED = 1 << 5,
    OVERFLOW = 1 << 6,
    NEGATIVE = 1 << 7,
}

