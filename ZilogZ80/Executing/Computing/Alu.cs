namespace ZilogZ80.Executing.Computing;

// ARITHMETIC LOGIC UNIT
public partial class Alu
{
    public AluOutput Compute(AluInput input, Operation operation)
    {
        AluOutput output = Operations[(byte)operation](input);
        
        if(output.Custom) return output;
        
        if ((output.Result & 0x80) != 0) output.Flags |= (byte)Flag.SIGN;
        if (output.Result == 0) output.Flags |= (byte)Flag.ZERO;
        if ((output.Result & 0x08) != 0) output.Flags |= (byte)Flag.BIT3;
        if ((output.Result & 0x20) != 0) output.Flags |= (byte)Flag.BIT5;

        return output;
    }
    
    private static readonly Func<AluInput, AluOutput>[] Operations =
    [
        NONE, ADD, ADC, SUB, SBC, ANA, XRA, ORA, INC, DEC,
        RLC, RRC, RAL, RAR, SLA, SLL, SRA, SRL, BIT, RES, SET,
        DAA, SCF, CCF, CPL, RFR, RST, TOP, IOP, IDX, SXT, BLK, VEC,
    ];
}

public enum Operation
{
    NONE, ADD, ADC, SUB, SBC, ANA, XRA, ORA, INC, DEC,
    RLC, RRC, RAL, RAR, SLA, SLL, SRA, SRL, BIT, RES, SET,
    DAA, SCF, CCF, CPL, RFR, RST, TOP, IOP, IDX, SXT, BLK, VEC,
}

public struct AluInput
{
    public byte A;
    public byte B;
    public byte C;
    public byte FL;
    public Flags FR;
}

public struct AluOutput
{
    public byte Result;
    public byte Flags;
    public bool Custom;
}