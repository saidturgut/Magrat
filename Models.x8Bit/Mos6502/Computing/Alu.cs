namespace Models.x8Bit.Mos6502.Computing;
using Engine.Units;

// ARITHMETIC LOGIC UNIT
public partial class Alu : x8Bit.Engine.Alu, IAlu
{
    public AluOutput Compute(AluInput input, byte operation)
    {
        AluOutput output = Operations[operation](input);
        
        if (output.Custom) return output;
        
        if ((output.Result & 0x80) != 0) output.Flags |= (byte)Flag.NEGATIVE;
        if (output.Result == 0) output.Flags |= (byte)Flag.ZERO;

        return output;
    }
    
    private static readonly Func<AluInput, AluOutput>[] Operations =
    [
        NONE, 
        ADC, SBC, CMP, AND, OR, EOR, INC, DEC,
        ASL, LSR, ROL, ROR, BIT,
        CLR, SET, PSR, SRP, IDX, CRY, SXT,
    ];
}

public enum Operation
{
    NONE, 
    ADC, SBC, CMP, AND, OR, EOR, INC, DEC,
    ASL, LSR, ROL, ROR, BIT,
    CLR, SET, PSR, SRP, IDX, CRY, SXT,
}
