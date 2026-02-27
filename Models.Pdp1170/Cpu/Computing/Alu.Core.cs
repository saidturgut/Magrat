namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput ZRO(AluInput input) => new()
        { Result = 0x0000, Flags = 0x0000, Custom = true };
    private static AluOutput SET(AluInput input) => new() 
        { Result = 0xFFFF, Flags = 0xFFFF, Custom = true };
    
    private static AluOutput ICC(AluInput input)
        => new() { Result = (ushort)(input.A + (input.ByteMode ? 1 : 2)) };
    private static AluOutput DCC(AluInput input)
        => new() { Result = (ushort)(input.A - (input.ByteMode ? 1 : 2)) };
    private static AluOutput IDX(AluInput input)
        => new() { Result = (ushort)(input.A + (short)input.B) };
    
    private static AluOutput BRC(AluInput input)
        => new() { Result = (ushort)(input.A + ((sbyte)(input.B & 0xFF) << 1)) };

    private static AluOutput TRP(AluInput input)
    {
        AluOutput output = new();
        if ((input.Psw & (ushort)Flag.CM0) != 0) 
            output.Flags |= (ushort)Flag.PM0;
        if ((input.Psw & (ushort)Flag.CM1) != 0) 
            output.Flags |= (ushort)Flag.PM1;

        output.Flags |= (ushort)Flag.CM0;
        output.Flags |= (ushort)Flag.CM1;
        return output;
    }
    
    private static ushort MaskHigh(ushort result)
        => (ushort)(result & xFFFF);

    private static byte CarryIn(Flags flags)
        => (byte)(flags.Carry ? 1 : 0);
    
    private static bool OverflowAdd(ushort a, ushort b, ushort result)
        => (~(a ^ b) & (a ^ result) & x8000) != 0;
    private static bool OverflowSub(ushort a, ushort b, ushort result)
        => ((a ^ b) & (a ^ result) & x8000) != 0;
    
    private static bool CarryBit(ushort a, ushort mask)
        => (a & mask) != 0;
    private static bool OverflowBit(ushort result, bool carry)
        => ((result & x8000) != 0) ^ carry;
}