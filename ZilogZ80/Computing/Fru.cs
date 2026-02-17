namespace ZilogZ80.Computing;
using Kernel.Devices;

// FLAGS REGISTER UNIT
public class Fru : IFru
{
    public bool Check(byte condition, Flags tmp) => condition switch
    {
        0 => !tmp.Bit6,
        1 => tmp.Bit6,
        2 => !tmp.Bit0,
        3 => tmp.Bit0,
        4 => !tmp.Bit2,
        5 => tmp.Bit2,
        6 => !tmp.Bit7,
        7 => tmp.Bit7,
        8 => !tmp.Bit2 && !tmp.Bit6,
        _ => true,
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
