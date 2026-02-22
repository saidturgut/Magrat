namespace Models.Pdp1170.Cpu.Computing;

public class Psw
{
}

public struct Flags(ushort psw)
{
    public bool Carry = (psw & (ushort)Flag.CARRY) != 0;
    public bool Overflow = (psw & (ushort)Flag.OVERFLOW) != 0;
    public bool Zero = (psw & (ushort)Flag.ZERO) != 0;
    public bool Negative = (psw & (ushort)Flag.NEGATIVE) != 0;
    public bool Trace = (psw & (ushort)Flag.TRACE) != 0;
}

public struct StatusWord(ushort psw)
{
    public ushort Priority = (ushort)((psw >> 7) & 0b111);
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

public enum Mode
{
    KERNEL, SUPERVISOR, RESERVED, USER,
}
