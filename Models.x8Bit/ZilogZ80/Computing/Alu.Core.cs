namespace Models.x8Bit.ZilogZ80.Computing;
using Engine.Units;

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
        if (SignedOverflowAdd(input.A, input.B, output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        if (HalfCarryAdd(input.A, input.B, input.C)) output.Flags |= (byte)Flag.HALF;

        return output;
    }
    private static AluOutput SBC(AluInput input)
    {
        var result = input.A + ~input.B + (1 - input.C);

        AluOutput output = new()
            { Result = (byte)result };

        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowSub(input.A, input.B, output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        if (HalfCarrySub(input.A, input.B, input.C)) output.Flags |= (byte)Flag.HALF;
        output.Flags |= (byte)Flag.SUBTRACT;

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
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        output.Flags |= (byte)Flag.HALF;
        return output;
    }
    
    private static AluOutput XRA(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A ^ input.B), };
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }

    private static AluOutput ORA(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A | input.B), };
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
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
}