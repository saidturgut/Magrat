namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput ZRO(AluInput input) => new()
        { Result = 0x0000, Flags = 0x0000 };
    private static AluOutput SET(AluInput input) => new() 
        { Result = 0xFFFF, Flags = 0xFFFF };
    
    private static AluOutput ICC(AluInput input)
        => new() { Result = (ushort)(input.A + (input.ByteMode ? 1 : 2)) };
    private static AluOutput DCC(AluInput input)
        => new() { Result = (ushort)(input.A - (input.ByteMode ? 1 : 2)) };
    private static AluOutput IDX(AluInput input)
        => new() { Result = (ushort)(input.A + (short)input.B) };
    
    private static AluOutput BRC(AluInput input)
        => new() { Result = (ushort)(input.A + ((sbyte)(input.B & 0xFF) << 1)) };
    
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