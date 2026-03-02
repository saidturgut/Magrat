using System.Data;

namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static ushort xFFFF;
    private static ushort x8000;
    private static uint x10000;

    public AluOutput Compute(AluInput input, Operation operation)
    {
        xFFFF = (ushort)(!input.ByteMode ? 0xFFFF : 0xFF);
        x8000 = (ushort)(!input.ByteMode ? 0x8000 : 0x80);
        x10000 = (uint)(!input.ByteMode ? 0x10000 : 0x100);
        
        var output = SelectOperation(input, operation);

        if (output.Custom) return output;
        
        if((output.Result & x8000) != 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if(output.Result == 0) output.Flags |= (ushort)Flag.ZERO;

        if (input.ByteMode)
        {
            output.Result = output.ByteMask switch
            {
                ByteMask.NONE => output.Result,
                ByteMask.PRES_HIGH => (ushort)((input.A & 0xFF00) | output.Result),
                ByteMask.EXT_SIGN => (ushort)((sbyte)output.Result)
            };
        }
        return output;
    }

    private AluOutput SelectOperation(AluInput input, Operation operation) => operation switch
    {
        Operation.ZRO => ZRO(input), Operation.SET => SET(input),
        Operation.ICC => ICC(input), Operation.DCC => DCC(input), Operation.IDX => IDX(input),
        Operation.PASS => PASS(input), Operation.ADD => ADD(input), Operation.SUB => SUB(input),
        Operation.BIT => BIT(input), Operation.BIC => BIC(input), Operation.BIS => BIS(input),
        Operation.CLR => CLR(input), Operation.COM => COM(input), Operation.NEG => NEG(input), Operation.TST => TST(input), 
        Operation.INC => INC(input), Operation.DEC => DEC(input), Operation.ADC => ADC(input), Operation.SBC => SBC(input),
        Operation.ROR => ROR(input), Operation.ROL => ROL(input), Operation.ASR => ASR(input), Operation.ASL => ASL(input),
        Operation.SXT => SXT(input), Operation.SWAB => SWAB(input), 
        Operation.BRC => BRC(input), Operation.MARK => MARK(input), Operation.SOB => SOB(input),
        Operation.MUL_L => MUL_L(input), Operation.MUL_H => MUL_H(input),
        Operation.DIV_L => DIV_L(input), Operation.DIV_H => DIV_H(input),
        Operation.ASH_L => ASH_L(input), Operation.ASH_H => ASH_H(input),
        Operation.XOR => XOR(input), Operation.ASH => ASH(input),
    };
}

public struct AluInput(ushort a, ushort b, ushort c, ushort psw, bool byteMode)
{
    public ushort A = a;
    public ushort B = b;
    public readonly ushort C = c;
    public readonly ushort Psw = psw;
    public readonly bool ByteMode = byteMode;
}

public struct AluOutput
{
    public ushort Result;
    public ushort Flags;
    public bool Custom;
    public ByteMask ByteMask;
}

public enum ByteMask
{
    NONE, EXT_SIGN, PRES_HIGH
}
