namespace ZilogZ80.Executing.Computing;

public partial class Alu
{
    private static AluOutput RLC(AluInput input) => new() // RLCA
    {
        Result = (byte)((input.A << 1) | Bit7(input.A)),
        Flags = RotateCarry(Bit7(input.A))
    };
    private static AluOutput RAL(AluInput input) => new() // RLA
    {
        Result = (byte)((input.A << 1) | input.C),
        Flags = RotateCarry(Bit7(input.A))
    };
    private static AluOutput RRC(AluInput input) => new() // RRCA
    {
        Result = (byte)((input.A >> 1) | (Bit0(input.A) << 7)),
        Flags = RotateCarry(Bit0(input.A))
    };
    private static AluOutput RAR(AluInput input) => new() // RRA
    {
        Result = (byte)((input.A >> 1) | (input.C << 7)),
        Flags = RotateCarry(Bit0(input.A))
    };
    
    private static AluOutput DAA(AluInput input)
    {
        byte a = input.A;
        byte fixer = 0;
        bool carry = input.FR.Carry;

        if (!input.FR.Subt)
        {
            if (input.FR.Half || (a & 0x0F) > 9)
                fixer |= 0x06;

            if (carry || a > 0x99)
            {
                fixer |= 0x60;
                carry = true;
            }

            a += fixer;
        }
        else
        {
            if (input.FR.Half)
                fixer |= 0x06;

            if (carry)
                fixer |= 0x60;

            a -= fixer;
        }

        AluOutput output = new()
            { Result = a };
        
        if ((fixer & 0x06) != 0) output.Flags |= (byte)Flag.HALF;

        if (EvenParity(a)) output.Flags |= (byte)Flag.OVER;
        
        if (carry) output.Flags |= (byte)Flag.CARRY;

        return output;
    }

    
    private static byte RotateCarry(byte bit) => (byte)((byte)Flag.CARRY & bit);
    private static byte Bit7(byte A) => (byte)((A >> 7) & 1);
    private static byte Bit0(byte A) => (byte)(A & 1);
}