namespace ZilogZ80.Executing.Computing;

// FLAGS REGISTER UNIT
public class Fru
{
    public Flags Flags;

    public void Update(byte sr)
    {
        Flags.Carry = (sr & (byte)Flag.CARRY) != 0; // C
        Flags.Subt = (sr & (byte)Flag.SUBT) != 0; // N
        Flags.Over = (sr & (byte)Flag.OVER) != 0; // P
        Flags.Bit3 = (sr & (byte)Flag.BIT3) != 0; // bit 3 of result
        Flags.Half = (sr & (byte)Flag.HALF) != 0; // H
        Flags.Bit5 = (sr & (byte)Flag.BIT5) != 0; // bit 5 of result
        Flags.Zero = (sr & (byte)Flag.ZERO) != 0; // Z
        Flags.Sign = (sr & (byte)Flag.SIGN) != 0; // S
    }
    
    public bool Check(Condition condition) => condition switch
    {
        Condition.NONE => true,
        Condition.NZ => !Flags.Zero,
        Condition.Z => Flags.Zero,
        Condition.NC => !Flags.Carry,
        Condition.C => Flags.Carry,
        Condition.PO => !Flags.Over,
        Condition.PE => Flags.Over,
        Condition.P => !Flags.Sign,
        Condition.M => Flags.Sign,
    };
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

public enum Condition
{
    NONE, NZ, Z, NC, C, PO, PE, P, M,
}