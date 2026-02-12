namespace ZilogZ80.Executing.Computing;

// STATUS REGISTER UNIT
public class Sru
{
    public bool Carry;
    public bool Last;
    public bool Parity;
    public bool Bit3;
    public bool Half;
    public bool Bit5;
    public bool Zero;
    public bool Sign;

    public void Update(byte sr)
    {
        Carry = (byte)(sr & (byte)Flag.CARRY) != 0; // C
        Last = (byte)(sr & (byte)Flag.LAST) != 0; // N
        Parity = (byte)(sr & (byte)Flag.PARITY) != 0; // P
        Bit3 = (byte)(sr & (byte)Flag.BIT3) != 0; // bit 3 of result
        Half = (byte)(sr & (byte)Flag.HALF) != 0; // H
        Bit5 = (byte)(sr & (byte)Flag.BIT5) != 0; // bit 5 of result
        Zero = (byte)(sr & (byte)Flag.ZERO) != 0; // Z
        Sign = (byte)(sr & (byte)Flag.SIGN) != 0; // S
    }
}

[Flags]
public enum Flag
{
    NONE = -1,
    CARRY = 1 << 0,
    LAST = 1 << 1,
    PARITY = 1 << 2,
    BIT3 = 1 << 3,
    HALF = 1 << 4,
    BIT5 = 1 << 5,
    ZERO = 1 << 6,
    SIGN = 1 << 7,
}
