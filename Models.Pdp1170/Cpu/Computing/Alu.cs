namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    public AluOutput Compute(AluInput input, Operation operation)
    {
        AluOutput output = SelectOperation(input, operation);
        
        if (input.ByteMode) output.Result &= 0xFF;
        if((output.Result & input.x8000) != 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if(output.Result == 0) output.Flags |= (ushort)Flag.ZERO;
        return output;
    }

    private AluOutput SelectOperation(AluInput input, Operation operation) => operation switch
    {
        Operation.PASS => PASS(), Operation.ICC => ICC(input), Operation.DCC => DCC(input),
        Operation.ADD => ADD(input), Operation.SUB => SUB(input),
        Operation.BIT => BIT(input), Operation.BIC => BIC(input), Operation.BIS => BIS(input)
    };
}

public struct AluInput(ushort a, ushort b, bool byteMode)
{
    public readonly ushort A = a;
    public readonly ushort B = b;
    public readonly bool ByteMode = byteMode;
    public readonly ushort xFFFF = (ushort)(!byteMode ? 0xFFFF : 0xFF);
    public readonly ushort x8000 = (ushort)(!byteMode ? 0x8000 : 0x80);
    public readonly uint x10000 = (uint)(!byteMode ? 0x10000 : 0x100);
}

public struct AluOutput
{
    public ushort Result;
    public ushort Flags;
}
