namespace Motorola6809.Computing;
using Kernel.Devices;

// FLAGS REGISTER UNIT
public class Fru : IFru
{
    public bool Check(byte condition, Flags tmp) => condition switch
    {
        _ => true
    };
}

[Flags]
public enum Flag
{
    NONE = 0,
    CARRY = 1 << 0,
    OVERFLOW = 1 << 1,
    ZERO = 1 << 2,
    NEGATIVE = 1 << 3,
    IRQ = 1 << 4,
    HALF = 1 << 5,
    FIRQ = 1 << 6,
    ENTIRE = 1 << 7,
}

