namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static readonly Pointer[] PC = [Pointer.PCL, Pointer.PCH];
    private static readonly Pointer[] SP = [Pointer.SPL, Pointer.SPH];
    private static readonly Pointer[] WZ = [Pointer.W, Pointer.Z];
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.IR),
        ALU_COMPUTE(Operation.RFR, Pointer.RR, Pointer.NIL, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.RR),
        STATE_COMMIT(Cycle.DECODE),
    ];
    
    public static Signal[] PREFIX =>
    [
        ..FETCH,
    ];
    
    public static Signal[] PREFIX_IND_BIT =>
    [
        ..FETCH,
        REG_COMMIT(Pointer.MDR, Pointer.TMP),
    ];

    public static readonly Signal[] SAVE =
    [
        REG_COMMIT(Pointer.IR, Pointer.TMP),
    ];
    
    private static Signal[] READ_IMM =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
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

    private static Signal PAIR_INC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_INC, First = pair[0], };
    private static Signal PAIR_DEC(Pointer[] pair) => new()
        { Cycle = Cycle.PAIR_DEC, First = pair[0] };
    
    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
    };

}

public enum FlagMask
{
}
