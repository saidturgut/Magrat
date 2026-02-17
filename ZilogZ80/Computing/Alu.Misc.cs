namespace ZilogZ80.Computing;
using Kernel.Devices;

public partial class Alu
{
    private static AluOutput RFR(AluInput input) => new()
        { Result = (byte)((input.A & 0x80) | (byte)((input.A + 1) & 0x7F)), };
    
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
        { Result = (byte)(((input.A >> 3) & 0x07) << 3), };

    private static AluOutput TOP(AluInput input) => new()
        { Result = 0xFF };
    private static AluOutput IOP(AluInput input) => new()
        { Flags = (byte)(EvenParity(input.A) ? (byte)Flag.OVER : 0) };
    private static AluOutput BLK(AluInput input) => new()
        { Flags = (byte)(input.A != 0 || input.B != 0 ? (byte)Flag.OVER : 0) };
    private static AluOutput VEC(AluInput input) => new() 
        { Result = 0x38, };
}