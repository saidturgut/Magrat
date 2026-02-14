namespace ZilogZ80.Executing.Computing;

public partial class Alu
{
    private static AluOutput ShiftRotate(byte result, byte carry) => new()
    {
        Result = result,
        Flags = (byte)(RotateCarry(carry) | (EvenParity(result) ? (byte)Flag.OVER : 0))
    };

    private static AluOutput RLC(AluInput input) 
        => ShiftRotate((byte)((input.A << 1) | Bit7(input.A)), Bit7(input.A));
    private static AluOutput RAL(AluInput input) 
        => ShiftRotate((byte)((input.A << 1) | input.C), Bit7(input.A));
    private static AluOutput RRC(AluInput input) 
        => ShiftRotate((byte)((input.A >> 1) | (Bit0(input.A) << 7)), Bit0(input.A));
    private static AluOutput RAR(AluInput input) 
        => ShiftRotate((byte)((input.A >> 1) | (input.C << 7)), Bit0(input.A));

    private static AluOutput SLA(AluInput input)
        => ShiftRotate((byte)(input.A << 1), Bit7(input.A));
    private static AluOutput SLL(AluInput input)
        => ShiftRotate((byte)((input.A << 1) | 1), Bit7(input.A));
    private static AluOutput SRA(AluInput input)
        => ShiftRotate((byte)((input.A >> 1) | (input.A & 0x80)), Bit0(input.A));
    private static AluOutput SRL(AluInput input)
        => ShiftRotate((byte)(input.A >> 1), Bit0(input.A));
    
    private static AluOutput RES(AluInput input) => new()
        { Result = (byte)(input.A & ~(1 << BitIndex(input.B))) };
    private static AluOutput SET(AluInput input) => new()
        { Result = (byte)(input.A | (1 << BitIndex(input.B))) };
    
    private static AluOutput BIT(AluInput input)
    {
        byte bitIndex = BitIndex(input.B);
        bool bitSet = ((input.A >> bitIndex) & 1) != 0;

        AluOutput output = new() { Custom = true, };
        
        if (!bitSet) output.Flags |= (byte)Flag.ZERO;
        if (!bitSet) output.Flags |= (byte)Flag.OVER;
        output.Flags |= (byte)Flag.HALF;
        if (bitIndex == 7 && bitSet) output.Flags |= (byte)Flag.SIGN;
        output.Flags |= (byte)(input.A & ((byte)Flag.BIT3 | (byte)Flag.BIT5));

        return output;
    }
    
    private static AluOutput DAA(AluInput input)
    {
        byte a = input.A;
        byte fixer = 0;
        bool carry = input.FR.Carry;

        if (!input.FR.Subt)
        {
            if (input.FR.Half || (a & 0x0F) > 9) fixer |= 0x06;
            if (carry || a > 0x99)
            {
                fixer |= 0x60;
                carry = true;
            }
            a += fixer;
        }
        else
        {
            if (input.FR.Half) fixer |= 0x06;
            if (carry) fixer |= 0x60;
            a -= fixer;
        }

        AluOutput output = new() { Result = a };
        if ((fixer & 0x06) != 0) output.Flags |= (byte)Flag.HALF;
        if (EvenParity(a)) output.Flags |= (byte)Flag.OVER;
        if (carry) output.Flags |= (byte)Flag.CARRY;
        return output;
    }
    
    private static byte BitIndex(byte input) => (byte)((input >> 3) & 7);
    private static byte RotateCarry(byte carry) => (byte)((byte)Flag.CARRY & carry);
    private static byte Bit7(byte A) => (byte)((A >> 7) & 1);
    private static byte Bit0(byte A) => (byte)(A & 1);
}