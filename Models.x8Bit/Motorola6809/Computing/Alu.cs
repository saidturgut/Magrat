namespace Models.x8Bit.Motorola6809.Computing;
using Engine.Units;

// ARITHMETIC LOGIC UNIT
public partial class Alu : Engine.Alu, IAlu
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
        ADD, ADC, SUB, SBC, AND, OR, EOR, 
        LSR, LSL, ASR, ROR, ROL, NEG, COM, INC, DEC,
  ];
}

public enum Operation
{
    NONE, 
    ADD, ADC, SUB, SBC, AND, OR, EOR, 
    LSR, LSL, ASR, ROR, ROL, NEG, COM, INC, DEC,
}
