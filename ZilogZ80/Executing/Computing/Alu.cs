namespace ZilogZ80.Executing.Computing;

// ARITHMETIC LOGIC UNIT
public partial class Alu
{
    public AluOutput Compute(AluInput input, Operation operation)
    {
        AluOutput output = Operations[(byte)operation](input);
        
        if ((output.Result & 0x80) != 0) output.Flags |= (byte)Flag.SIGN;
        if (output.Result == 0) output.Flags |= (byte)Flag.ZERO;
        if ((output.Result & 0x08) != 0) output.Flags |= (byte)Flag.BIT3;
        if ((output.Result & 0x20) != 0) output.Flags |= (byte)Flag.BIT5;

        return output;
    }
    
    private static readonly Func<AluInput, AluOutput>[] Operations =
    [
        NONE, ADD, ADC, SUB, SBC, ANA, XRA, ORA, INC, DEC,
        RLC, RRC, RAL, RAR, DAA, SCF, CCF, CPL,
        RFR, 
    ];
}

public enum Operation
{
    NONE, ADD, ADC, SUB, SBC, ANA, XRA, ORA, INC, DEC,
    RLC, RRC, RAL, RAR, DAA, SCF, CCF, CPL,
    RFR, 
}

public struct AluInput
{
    public byte A;
    public byte B;
    public byte C;
    public Flags F;
}

public struct AluOutput
{
    public byte Result;
    public byte Flags;
    public bool Custom;
}