namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput ICC(AluInput input)
        => new() { Result = (ushort)(input.A + (input.ByteMode ? 1 : 2)) };
    private static AluOutput DCC(AluInput input)
        => new() { Result = (ushort)(input.A - (input.ByteMode ? 1 : 2)) };

    private static bool Carry(AluInput input, uint sum) 
        => (sum & input.x10000) != 0;
    private static bool OverflowAdd(AluInput input, ushort result)
        => (~(input.A ^ input.B) & (input.A ^ result) & input.x8000) != 0;
    private static bool OverflowSub(AluInput input, ushort result)
        => ((input.A ^ input.B) & (input.A ^ result) & input.x8000) != 0;
}