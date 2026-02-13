namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static readonly Pointer[] PC = [Pointer.PCL, Pointer.PCH];
    private static readonly Pointer[] SP = [Pointer.SPL, Pointer.SPH];
    private static readonly Pointer[] HL = [Pointer.L, Pointer.H];
    private static readonly Pointer[] WZ = [Pointer.W, Pointer.Z];
    private static readonly Pointer[] BC = [Pointer.C, Pointer.B];
    private static readonly Pointer[] DE = [Pointer.E, Pointer.D];

    private static Signal[] IDLE =>
        [STATE_COMMIT(Cycle.IDLE)];
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.IR),
        ALU_COMPUTE(Operation.RFR, Pointer.RR, Pointer.NIL, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.RR),
        STATE_COMMIT(Cycle.DECODE),
    ];
    
    public static Signal[] DISP =>
    [
        REG_COMMIT(Pointer.IR, Pointer.W),
        ..FETCH,
    ];
    
    private static Signal STATE_COMMIT(Cycle state) => new()
        { Cycle = state, };
    
    private static Signal REG_COMMIT(Pointer source, Pointer destination) => new()
        { Cycle = Cycle.REG_COMMIT, First = source, Second = destination };

    private static Signal MEM_READ(Pointer[] address) => new()
        { Cycle = Cycle.MEM_READ, First = address[0], Second = address[1] };
    private static Signal MEM_WRITE(Pointer[] address) => new()
        { Cycle = Cycle.MEM_WRITE, First = address[0], Second = address[1] };

    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, First = source, Second = operand, Mask =  mask };
    private static Signal COND_COMPUTE(Condition condition) => new()
        { Cycle = Cycle.IDLE, Condition = condition };

    private static Signal PAIR_INC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_INC, First = pair[0], Second = pair[1] };
    private static Signal PAIR_DEC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_DEC, First = pair[0], Second = pair[1] };
    
    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
        { FlagMask.CNV3H5ZS , Flag.CARRY | Flag.SUBT | Flag.OVER | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.CV3H5ZS , Flag.CARRY | Flag.OVER | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.NV3H5ZS , Flag.SUBT | Flag.OVER | Flag.BIT3 | Flag.HALF | Flag.BIT5 | Flag.ZERO | Flag.SIGN },
        { FlagMask.CN3H5 , Flag.CARRY | Flag.SUBT | Flag.BIT3 | Flag.HALF | Flag.BIT5 },
        { FlagMask.N3H5 , Flag.SUBT | Flag.BIT3 | Flag.HALF | Flag.BIT5 },
        { FlagMask.CNH , Flag.CARRY | Flag.SUBT | Flag.HALF },
    };
}

public enum FlagMask
{
    CNV3H5ZS, CV3H5ZS, NV3H5ZS, CN3H5, N3H5, CNH
}
