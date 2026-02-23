namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    public AluOutput Compute(AluInput input, Operation operation)
    {
        AluOutput output = SelectOperation(input, operation);
        
        ushort result = (ushort)(output.Result & input.xFFFF);
        if((result & input.x8000) != 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if(result == 0) output.Flags |= (ushort)Flag.ZERO;

        if (input.ByteMode)
            output.Result = output.ByteMask switch
            {
                ByteMask.NONE => output.Result,
                ByteMask.MASK_HIGH => (ushort)((input.A & 0xFF00) | result),
                ByteMask.EXT_SIGN => output.Result = (ushort)((sbyte)result)
            };
        return output;
    }

    private AluOutput SelectOperation(AluInput input, Operation operation) => operation switch
    {
        Operation.NONE => new AluOutput(),
        Operation.ICC => ICC(input), Operation.DCC => DCC(input), Operation.IDX => IDX(input),
        Operation.PASS => PASS(input), Operation.ADD => ADD(input), Operation.SUB => SUB(input),
        Operation.BIT => BIT(input), Operation.BIC => BIC(input), Operation.BIS => BIS(input),
        _=> new AluOutput(),
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
    public ByteMask ByteMask;
}

public enum ByteMask
{
    NONE, EXT_SIGN, MASK_HIGH
}
