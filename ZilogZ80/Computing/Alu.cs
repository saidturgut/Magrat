namespace ZilogZ80.Computing;
using Kernel.Devices;

// ARITHMETIC LOGIC UNIT
public partial class Alu : IAlu
{
    public AluOutput Compute(AluInput input, byte operation)
    {
        AluOutput output = Operations[operation](input);
        
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
        DAA, SCF, CCF, CPL, RFR, RST, TOP, IOP, BLK, VEC,
        IDX, SXT, RRL, RRH, RLL, RLH,
    ];
}

public enum Operation
{
    NONE, ADD, ADC, SUB, SBC, ANA, XRA, ORA, INC, DEC,
    RLC, RRC, RAL, RAR, SLA, SLL, SRA, SRL, BIT, RES, SET,
    DAA, SCF, CCF, CPL, RFR, RST, TOP, IOP, BLK, VEC,
    IDX, SXT, RRL, RRH, RLL, RLH,
}
