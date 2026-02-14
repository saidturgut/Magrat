namespace ZilogZ80.Executing.Computing;

// FLAGS REGISTER UNIT
public class Fru
{
    public Flags Flags(byte source) => new ()
    {
        Carry = (source & (byte)Flag.CARRY) != 0,
        Subt = (source & (byte)Flag.SUBT) != 0,
        Over = (source & (byte)Flag.OVER) != 0,
        Bit3 = (source & (byte)Flag.BIT3) != 0,
        Half = (source & (byte)Flag.HALF) != 0,
        Bit5 = (source & (byte)Flag.BIT5) != 0,
        Zero = (source & (byte)Flag.ZERO) != 0,
        Sign = (source & (byte)Flag.SIGN) != 0,
    };
    
    public bool Check(Condition condition, Flags tmp) => condition switch
    {
        Condition.NONE => false,
        Condition.NZ => !tmp.Zero,
        Condition.Z => tmp.Zero,
        Condition.NC => !tmp.Carry,
        Condition.C => tmp.Carry,
        Condition.PO => !tmp.Over,
        Condition.PE => tmp.Over,
        Condition.P => !tmp.Sign,
        Condition.M => tmp.Sign,
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