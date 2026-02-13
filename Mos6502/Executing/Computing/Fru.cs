namespace Mos6502.Executing.Computing;

// FLAGS REGISTER UNIT
public class Fru
{
    public bool Carry;
    public bool Zero;
    public bool Interrupt;
    public bool Decimal;
    public bool Break;
    public bool Unused;
    public bool Overflow;
    public bool Negative;

    public void Update(byte sr)
    {
        Carry = (sr & (byte)Flag.CARRY) != 0;
        Zero = (sr & (byte)Flag.ZERO) != 0;
        Interrupt = (sr & (byte)Flag.INTERRUPT) != 0;
        Decimal = (sr & (byte)Flag.DECIMAL) != 0;
        Break = (sr & (byte)Flag.BREAK) != 0;
        Unused = (sr & (byte)Flag.UNUSED) != 0;
        Overflow = (sr & (byte)Flag.OVERFLOW) != 0;
        Negative = (sr & (byte)Flag.NEGATIVE) != 0;
    }
    
    public bool Check(Condition condition) => condition switch
    {
        Condition.NONE => true,
        Condition.CC => !Carry,
        Condition.CS => Carry,
        Condition.NE => !Zero,
        Condition.EQ => Zero,
        Condition.VC => !Overflow,
        Condition.VS => Overflow,
        Condition.PL => !Negative,
        Condition.MI => Negative,
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

public enum Condition
{       // CARRY, ZERO, OVERFLOW, NEGATIVE
    NONE, CC, CS, NE, EQ, VC, VS, PL, MI,
}