namespace Mos6502.Executing.Computing;

// FLAGS REGISTER UNIT
public class Fru
{
    public Flags Flags(byte source) => new ()
    {
        Carry = (source & (byte)Flag.CARRY) != 0,
        Zero = (source & (byte)Flag.ZERO) != 0,
        Interrupt = (source & (byte)Flag.INTERRUPT) != 0,
        Decimal = (source & (byte)Flag.DECIMAL) != 0,
        Break = (source & (byte)Flag.BREAK) != 0,
        Unused = (source & (byte)Flag.UNUSED) != 0,
        Overflow = (source & (byte)Flag.OVERFLOW) != 0,
        Negative = (source & (byte)Flag.NEGATIVE) != 0,
    };

    public bool Check(Condition condition, Flags tmp) => condition switch
    {
        Condition.NONE => true,
        Condition.NE => !tmp.Zero,
        Condition.EQ => tmp.Zero,
        Condition.CC => !tmp.Carry,
        Condition.CS => tmp.Carry,
        Condition.VC => !tmp.Overflow,
        Condition.VS => tmp.Overflow,
        Condition.PL => !tmp.Negative,
        Condition.MI => tmp.Negative,
    };
}

public struct Flags
{
    public bool Carry;
    public bool Zero;
    public bool Interrupt;
    public bool Decimal;
    public bool Break;
    public bool Unused;
    public bool Overflow;
    public bool Negative;
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

public enum Condition
{
    NONE, NE, EQ, CC, CS, VC, VS, PL, MI,
}