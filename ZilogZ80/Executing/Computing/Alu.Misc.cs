namespace ZilogZ80.Executing.Computing;

public partial class Alu
{
    private static AluOutput SCF(AluInput input)
    {
        AluOutput output = new();
        output.Flags |= (byte)Flag.CARRY;
        return output;
    }
    
    private static AluOutput CCF(AluInput input)
    {
        AluOutput output = new();
        if((input.C & (byte)Flag.CARRY) == 0) output.Flags |= (byte)Flag.CARRY;
        if((input.C & (byte)Flag.CARRY) != 0) output.Flags |= (byte)Flag.HALF;
        return output;
    }
    
    private static AluOutput CPL(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)~input.A, };
        output.Flags |= (byte)Flag.HALF;
        output.Flags |= (byte)Flag.SUBT;
        return output;
    }
    
    private static AluOutput RST(AluInput input) => new()
    {
        Result = (byte)(((input.A >> 3) & 0x07) << 3),
    };

    private static AluOutput TOP(AluInput input) => new()
    {
        Result = 0xFF 
    };
    
    private static AluOutput IDX(AluInput input)
    {
        var result = input.A + input.B;
        AluOutput output = new() { Result = (byte)result };
        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        return output;
    }

    private static AluOutput SXT(AluInput input) => new()
        { Result = (byte)(input.A + (byte)((input.B & 0x80) != 0 ? 0xFF : 0x00) + (input.FL & (byte)Flag.CARRY)) };
}