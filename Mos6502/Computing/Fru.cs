namespace Mos6502.Computing;
using Kernel.Devices;

// FLAGS REGISTER UNIT
public class Fru : IFru
{
    public Flags Flags(byte source) => new ()
    {
        Bit0 = (source & (byte)Flag.CARRY) != 0,
        Bit1 = (source & (byte)Flag.ZERO) != 0,
        Bit2 = (source & (byte)Flag.INTERRUPT) != 0,
        Bit3 = (source & (byte)Flag.DECIMAL) != 0,
        Bit4 = (source & (byte)Flag.BREAK) != 0,
        Bit5 = (source & (byte)Flag.UNUSED) != 0,
        Bit6 = (source & (byte)Flag.OVERFLOW) != 0,
        Bit7 = (source & (byte)Flag.NEGATIVE) != 0,
    };

    public bool Check(byte condition, Flags tmp) => condition switch
    {
        0 => true,
        1 => !tmp.Bit1,
        2 => tmp.Bit1,
        3 => !tmp.Bit0,
        4 => tmp.Bit0,
        5 => !tmp.Bit6,
        6 => tmp.Bit6,
        7 => !tmp.Bit7,
        8 => tmp.Bit7,
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
