namespace ZilogZ80.Computing;
using Kernel.Devices;

// FLAGS REGISTER UNIT
public class Fru : IFru
{
    public Flags Flags(byte source) => new ()
    {
        Bit0 = (source & (byte)Flag.CARRY) != 0,
        Bit1 = (source & (byte)Flag.SUBT) != 0,
        Bit2 = (source & (byte)Flag.OVER) != 0,
        Bit3 = (source & (byte)Flag.BIT3) != 0,
        Bit4 = (source & (byte)Flag.HALF) != 0,
        Bit5 = (source & (byte)Flag.BIT5) != 0,
        Bit6 = (source & (byte)Flag.ZERO) != 0,
        Bit7 = (source & (byte)Flag.SIGN) != 0,
    };

    public bool Check(byte condition, Flags tmp) => condition switch
    {
        0 => true,
        1 => !tmp.Bit6,
        2 => tmp.Bit6,
        3 => !tmp.Bit0,
        4 => tmp.Bit0,
        5 => !tmp.Bit2,
        6 => tmp.Bit2,
        7 => !tmp.Bit7,
        8 => tmp.Bit7,
        9 => !tmp.Bit2 && !tmp.Bit6,
    };
}

[Flags]
public enum Flag
{
    NONE = 0,
    CARRY = 1 << 0,
    SUBT = 1 << 1,
    OVER = 1 << 2,
    BIT3 = 1 << 3,
    HALF = 1 << 4,
    BIT5 = 1 << 5,
    ZERO = 1 << 6,
    SIGN = 1 << 7,
}
