namespace Models.x8Bit.Motorola6809.Decoding;
using Computing;
using Engine;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static readonly Pointer[] PC = [Pointer.PCL, Pointer.PCH];
    private static readonly Pointer[] WZ = [Pointer.W, Pointer.Z];
    private static readonly Pointer[] MDR = [Pointer.MDR, Pointer.NIL];
    private static readonly Pointer[] TMP = [Pointer.TMP, Pointer.NIL];
    private static readonly Pointer[] IX = [Pointer.IXL, Pointer.IXH];
    private static readonly Pointer[] IY = [Pointer.IYL, Pointer.IYH];
    private static readonly Pointer[] UP = [Pointer.UPL, Pointer.UPH];
    private static readonly Pointer[] SP = [Pointer.SPL, Pointer.SPH];
    private static readonly Pointer[] DD = [Pointer.B, Pointer.A];
    private static readonly Pointer[] DP = [Pointer.MDR, Pointer.DP];
    
    private static readonly Operation[] ADD16 = [Operation.ADD, Operation.ADC];
    private static readonly Operation[] SUB16 = [Operation.SUB, Operation.SBC];
    
    private static Signal STATE_COMMIT(State state) => new()
        { Cycle = Cycle.STATE_COMMIT, State = (byte)state, };
    
    private static Signal REG_COMMIT(Pointer source, Pointer destination) => new()
        { Cycle = Cycle.REG_COMMIT, First = (byte)source, Second = (byte)destination };

    private static Signal MEM_READ(Pointer[] address) => new()
        { Cycle = Cycle.MEM_READ, First = (byte)address[0], Second = (byte)address[1] };
    private static Signal MEM_WRITE(Pointer[] address) => new()
        { Cycle = Cycle.MEM_WRITE, First = (byte)address[0], Second = (byte)address[1] };

    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = (byte)operation, First = (byte)source, Second = (byte)operand, Mask =  (byte)mask };
/*    private static Signal COND_COMPUTE(State state, Condition condition) => new()
        { Cycle = Cycle.COND_COMPUTE, State = (byte)state, Condition = (byte)condition };
*/
    private static Signal PAIR_INC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_INC, First = (byte)pair[0], Second = (byte)pair[1] };
    private static Signal PAIR_DEC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_DEC, First = (byte)pair[0], Second = (byte)pair[1] };

    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
        { FlagMask.NONE, Flag.NONE },
        { FlagMask.CVZNIHFE, Flag.CARRY | Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE | Flag.IRQ | Flag.HALF | Flag.FIRQ | Flag.ENTIRE },
        { FlagMask.CVZNH, Flag.CARRY | Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE | Flag.HALF },
        { FlagMask.CVZN, Flag.CARRY | Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.VZN, Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.ZN, Flag.ZERO | Flag.NEGATIVE },
    };
}

public enum FlagMask
{
    NONE,
    CVZNIHFE, CVZNH, CVZN, VZN, ZN,
}
