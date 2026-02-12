namespace ZilogZ80.Executing.Computing;

// ARITHMETIC LOGIC UNIT
public partial class Alu
{
    public AluOutput Compute(AluInput input, Operation operation)
    {
        AluOutput output = Operations[(byte)operation](input);
        
        if (output.Custom) return output;
        
        if ((output.Result & 0x80) != 0) output.Flags |= (byte)Flag.SIGN;
        if (output.Result == 0) output.Flags |= (byte)Flag.ZERO;

        return output;
    }
    
    private static readonly Func<AluInput, AluOutput>[] Operations =
    [
        NONE, RFR,
    ];
}

public enum Operation
{
    NONE, RFR,
}

public struct AluInput
{
    public byte A;
    public byte B;
    public byte C;
    public byte F;
}

public struct AluOutput
{
    public byte Result;
    public byte Flags;
    public bool Custom;
}