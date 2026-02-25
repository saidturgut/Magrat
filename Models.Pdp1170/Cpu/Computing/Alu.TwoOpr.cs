namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput PASS(AluInput input) 
        => new() { Result = MaskHigh(input.B), ByteMask = ByteMask.EXT_SIGN };
    
    private static AluOutput ADD(AluInput input)
    {
        uint sum = (uint)(input.A + input.B);
        AluOutput output = new() { Result = (ushort)sum };
        if ((sum & x10000) != 0) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowAdd(input.A, input.B, output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput SUB(AluInput input)
    {
        input.A = MaskHigh(input.A);
        input.B = MaskHigh(input.B);
        AluOutput output = new() { Result = MaskHigh((ushort)(input.A - input.B)) };
        if (input.A < input.B) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowSub(input.A, input.B, output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput BIT(AluInput input) => new()
        { Result = MaskHigh((ushort)(MaskHigh(input.A) & MaskHigh(input.B))), ByteMask = ByteMask.PRES_HIGH };
    private static AluOutput BIC(AluInput input) => new() 
        { Result = MaskHigh((ushort)(MaskHigh(input.A) & ~MaskHigh(input.B))), ByteMask = ByteMask.PRES_HIGH };
    private static AluOutput BIS(AluInput input) => new() 
        { Result = MaskHigh((ushort)(MaskHigh(input.A) | MaskHigh(input.B))), ByteMask = ByteMask.PRES_HIGH };
}