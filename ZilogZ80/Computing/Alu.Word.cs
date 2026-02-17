namespace ZilogZ80.Computing;
using Kernel.Devices;

public partial class Alu
{
    private static AluOutput IDX(AluInput input)
    {
        var result = input.A + input.B;
        AluOutput output = new() { Result = (byte)result };
        if (Carry(result, 8)) output.Flags |= (byte)Flag.CARRY;
        return output;
    }
    
    private static AluOutput SXT(AluInput input) => new()
        { Result = (byte)(input.A + (byte)((input.B & 0x80) != 0 ? 0xFF : 0x00) + (input.FL & (byte)Flag.CARRY)) };
    
    private static AluOutput RRL(AluInput input)
    {
        byte result = (byte)((input.A & 0xF0) | (input.B & 0x0F));
        AluOutput output = new() { Result = result };
        if (EvenParity(result)) output.Flags |= (byte)Flag.OVER;
        return output;
    }
    private static AluOutput RRH(AluInput input) => new() 
        { Result = (byte)((input.B >> 4) | ((input.A & 0x0F) << 4)) };
    
    private static AluOutput RLL(AluInput input)
    {
        byte result = (byte)((input.A & 0xF0) | (input.B >> 4));
        AluOutput output = new() { Result = result };
        if (EvenParity(result)) output.Flags |= (byte)Flag.OVER;
        return output;
    }
    private static AluOutput RLH(AluInput input) => new() 
        { Result = (byte)((input.B << 4) | (input.A & 0x0F)) };
}