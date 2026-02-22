namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput PASS() => new();
    
    private static AluOutput ADD(AluInput input)
    {
        uint sum = (uint)(input.A + input.B);
        AluOutput output = new() { Result = (ushort)(sum & input.xFFFF) };
        
        if (Carry(input, sum)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowAdd(input, output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput SUB(AluInput input)
    {
        uint sum = (uint)(input.A - input.B);
        AluOutput output = new() { Result = (ushort)(sum & input.xFFFF) };

        if (!Carry(input, sum)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowSub(input, output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput BIT(AluInput input) => new() 
        { Result = (ushort)(input.A & input.B) };
    private static AluOutput BIC(AluInput input) => new() 
        { Result = (ushort)(input.A & ~input.B) };
    private static AluOutput BIS(AluInput input) => new() 
        { Result = (ushort)(input.A | input.B) };
}