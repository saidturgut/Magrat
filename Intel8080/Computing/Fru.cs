namespace Intel8080.Computing;
using Kernel.Devices;

public class Fru : ZilogZ80.Computing.Fru, IFru;

[Flags]
public enum Flag
{
    NONE = 0,
    CARRY = 1 << 0,
    BIT1 = 1 << 1,
    OVER = 1 << 2,
    BIT3 = 1 << 3,
    HALF = 1 << 4,
    BIT5 = 1 << 5,
    ZERO = 1 << 6,
    SIGN = 1 << 7,
}