namespace Models.x8Bit.Motorola6809.Computing;
using Engine.Units;

public partial class Alu
{
    private static AluOutput NONE(AluInput input) => new();

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

    private static AluOutput AND(AluInput input) => new()
        { Result = (byte)(input.A & input.B), };
    private static AluOutput OR(AluInput input) => new()
        { Result = (byte)(input.A | input.B), };
    private static AluOutput EOR(AluInput input) => new()
        { Result = (byte)(input.A ^ input.B), };
}