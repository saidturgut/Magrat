namespace Models.x8Bit.Mos6502.Computing;
using Engine.Units;

public partial class Alu
{
    private static AluOutput NONE(AluInput input) => new()
        { Result = input.A, };
    
    private static AluOutput ADC(AluInput input)
    {
        if (input.FR.Bit3) return DAD(input);
        
        var result = input.A + input.B + input.C;
        AluOutput output = new() { Result = (byte)result };

        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowAdd(input.A, input.B, output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        
        return output;
    }
    private static AluOutput SBC(AluInput input)
    {
        if (input.FR.Bit3) return DSB(input);

        var result = input.A + ~input.B + input.C;
        AluOutput output = new() { Result = (byte)result };
        
        if (!Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowSub(input.A, input.B, output.Result)) output.Flags |= (byte)Flag.OVERFLOW;
        
        return output;
    }
    
    private static AluOutput CMP(AluInput input)
    {
        var result = input.A + ~input.B + 1;
        AluOutput output = new() { Result = (byte)result };
        if (!Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        return output;
    }
    
    private static AluOutput AND(AluInput input) => new()
        { Result = (byte)(input.A & input.B) };
    private static AluOutput EOR(AluInput input) => new()
        { Result = (byte)(input.A ^ input.B) };
    private static AluOutput OR(AluInput input) => new()
        { Result = (byte)(input.A | input.B) };
    
    private static AluOutput INC(AluInput input) => new()
        { Result = (byte)(input.A + 1), };
    private static AluOutput DEC(AluInput input) => new()
        { Result = (byte)(input.A - 1), };
}