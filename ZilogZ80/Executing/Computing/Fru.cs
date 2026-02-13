namespace ZilogZ80.Executing.Computing;

// FLAGS REGISTER UNIT
public class Fru
{
    public Flags Flags = new ();

    public void Update(byte sr)
    {
        Flags.Carry = (byte)(sr & (byte)Flag.CARRY) != 0; // C
        Flags.Subt = (byte)(sr & (byte)Flag.SUBT) != 0; // N
        Flags.Over = (byte)(sr & (byte)Flag.OVER) != 0; // P
        Flags.Bit3 = (byte)(sr & (byte)Flag.BIT3) != 0; // bit 3 of result
        Flags.Half = (byte)(sr & (byte)Flag.HALF) != 0; // H
        Flags.Bit5 = (byte)(sr & (byte)Flag.BIT5) != 0; // bit 5 of result
        Flags.Zero = (byte)(sr & (byte)Flag.ZERO) != 0; // Z
        Flags.Sign = (byte)(sr & (byte)Flag.SIGN) != 0; // S
    }
}

public struct Flags
{
    public bool Carry;
    public bool Subt;
    public bool Over;
    public bool Bit3;
    public bool Half;
    public bool Bit5;
    public bool Zero;
    public bool Sign;
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
