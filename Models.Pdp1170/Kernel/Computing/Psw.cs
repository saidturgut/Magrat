namespace Models.Pdp1170.Kernel.Computing;

public class Psw
{
    public static bool CheckCondition(Condition condition, Flags tmp) => condition switch
    {
        Condition.R => true,
        Condition.NE => !tmp.Zero,
        Condition.EQ => tmp.Zero,
        Condition.GE => tmp.Negative == tmp.Overflow,
        Condition.LT => tmp.Negative != tmp.Overflow,
        Condition.GT => !tmp.Zero && tmp.Negative == tmp.Overflow,
        Condition.LE => tmp.Zero || tmp.Negative != tmp.Overflow,
        Condition.EMT => true,
        Condition.PL => !tmp.Negative,
        Condition.MI => tmp.Negative,
        Condition.HI => !tmp.Carry && !tmp.Zero,
        Condition.LOS => tmp.Carry || tmp.Zero,
        Condition.VC => !tmp.Overflow,
        Condition.VS => tmp.Overflow,
        Condition.CC => !tmp.Carry,
        Condition.CS => tmp.Carry,
    };
}

public struct Flags(ushort psw)
{
    public readonly bool Carry = (psw & (ushort)Flag.CARRY) != 0;
    public readonly bool Overflow = (psw & (ushort)Flag.OVERFLOW) != 0;
    public readonly bool Zero = (psw & (ushort)Flag.ZERO) != 0;
    public readonly bool Negative = (psw & (ushort)Flag.NEGATIVE) != 0;
}

public struct StatusWord(ushort psw)
{
    public readonly bool Trace = (psw & (ushort)Flag.TRACE) != 0;
    public readonly byte Priority = (byte)((psw >> 5) & 0b111);
    public readonly byte RegisterSet = (byte)((psw & (ushort)Flag.RS) != 0 ? 1 : 0);
    public readonly Mode PreviousMode = CheckMode((byte)((psw >> 12) & 0b11));
    public readonly Mode CurrentMode = CheckMode((byte)((psw >> 14) & 0b11));

    private static Mode CheckMode(byte mode) => mode switch
    {
        0b00 => Mode.KERNEL,
        0b01 => Mode.SUPERVISOR,
        0b10 => Mode.RESERVED,
        0b11 => Mode.USER,
    };
}
