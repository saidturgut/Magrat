namespace ZilogZ80.Computing;
using System.Numerics;
using Kernel.Devices;

public partial class Alu
{
    private static AluOutput NONE(AluInput input) => new()
        { Result = input.A, };
    
    private static AluOutput ADC(AluInput input)
    {
        var result = input.A + input.B + input.C;

        AluOutput output = new()
            { Result = (byte)result };

        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflow(input.A, input.B, output.Result)) output.Flags |= (byte)Flag.OVER;
        if (HalfCarryAdd(input.A, input.B, input.C)) output.Flags |= (byte)Flag.HALF;

        return output;
    }
    private static AluOutput SBC(AluInput input)
    {
        var result = input.A + ~input.B + (1 - input.C);

        AluOutput output = new()
            { Result = (byte)result };

        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflow(input.A, (byte)~input.B, output.Result)) output.Flags |= (byte)Flag.OVER;
        if (HalfCarrySub(input.A, input.B, input.C)) output.Flags |= (byte)Flag.HALF;
        output.Flags |= (byte)Flag.SUBT;

        return output;
    }

    private static AluOutput ADD(AluInput input)
    {
        input.C = 0; return ADC(input);
    }
    private static AluOutput SUB(AluInput input)
    {
        input.C = 0; return SBC(input);
    }
    
    private static AluOutput ANA(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A & input.B), };
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVER;
        output.Flags |= (byte)Flag.HALF;
        return output;
    }
    
    private static AluOutput XRA(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A ^ input.B), };
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVER;
        return output;
    }

    private static AluOutput ORA(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A | input.B), };
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVER;
        return output;
    }

    private static AluOutput INC(AluInput input)
    {
        input.B = 1; input.C = 0; 
        return ADC(input); 
    }
    private static AluOutput DEC(AluInput input)
    {
        input.B = 1; input.C = 0;
        return SBC(input); 
    }
    
    private static bool Carry(int source, byte bit)
        => (byte)((source >> bit) & 1) != 0;
    private static bool HalfCarryAdd(byte A, byte B, byte C)
        => (A & 0xF) + (B & 0xF) + C > 0xF;
    private static bool HalfCarrySub(byte A, byte B, byte C)
        => (A & 0xF) < ((B & 0xF) + C);
    private static bool SignedOverflow(byte A, byte B, byte result)
        => (~(A ^ B) & (A ^ result) & 0x80) != 0;
    private static bool EvenParity(byte result)
        => BitOperations.PopCount(result) % 2 == 0;
}