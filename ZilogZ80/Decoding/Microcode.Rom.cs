namespace ZilogZ80.Decoding;
using Computing;
using Kernel;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static readonly Pointer[] PC = [Pointer.PCL, Pointer.PCH];
    private static readonly Pointer[] SP = [Pointer.SPL, Pointer.SPH];
    private static readonly Pointer[] AF = [Pointer.A, Pointer.FR];
    private static readonly Pointer[] HL = [Pointer.L, Pointer.H];
    private static readonly Pointer[] WZ = [Pointer.W, Pointer.Z];
    private static readonly Pointer[] BC = [Pointer.C, Pointer.B];
    private static readonly Pointer[] DE = [Pointer.E, Pointer.D];
    private static readonly Pointer[] TMP = [Pointer.TMP, Pointer.NIL];
    
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
    private static Signal COND_COMPUTE(State state, Condition condition) => new()
        { Cycle = Cycle.COND_COMPUTE, State = (byte)state, Condition = (byte)condition };

    private static Signal PAIR_INC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_INC, First = (byte)pair[0], Second = (byte)pair[1] };
    private static Signal PAIR_DEC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_DEC, First = (byte)pair[0], Second = (byte)pair[1] };
    
    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
        { FlagMask.NONE , Flag.NONE },
        { FlagMask.CNV3H5ZS , Flag.CARRY | Flag.SUBTRACT | Flag.OVERFLOW | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.CV3H5ZS , Flag.CARRY | Flag.OVERFLOW | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.NV3H5ZS , Flag.SUBTRACT | Flag.OVERFLOW | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.N3H5ZS , Flag.SUBTRACT  | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.CN3H5 , Flag.CARRY | Flag.SUBTRACT | Flag.BIT3 | Flag.HALF | Flag.BIT5 },
        { FlagMask.NVHZS , Flag.SUBTRACT | Flag.OVERFLOW | Flag.HALF | Flag.ZERO | Flag.SIGN },
        { FlagMask.NV3H5 , Flag.SUBTRACT | Flag.OVERFLOW | Flag.BIT3 | Flag.HALF | Flag.BIT5 },
        { FlagMask.N3H5 , Flag.SUBTRACT | Flag.BIT3 | Flag.HALF | Flag.BIT5 },
        { FlagMask.CNH , Flag.CARRY | Flag.SUBTRACT | Flag.HALF },
        { FlagMask.NZ , Flag.SUBTRACT | Flag.ZERO },
    };
}

public enum FlagMask
{
    NONE,
    CNV3H5ZS, CV3H5ZS, NV3H5ZS, N3H5ZS, CN3H5, NVHZS,  NV3H5, N3H5, CNH, NZ,
}
