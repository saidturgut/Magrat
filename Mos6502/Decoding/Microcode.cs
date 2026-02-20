using Mos6502.Signaling;

namespace Mos6502.Decoding;
using Computing;
using Kernel;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static readonly Pointer[] PC = [Pointer.PCL, Pointer.PCH];
    private static readonly Pointer[] SP = [Pointer.SPL, Pointer.SPH];
    private static readonly Pointer[] WZ = [Pointer.W, Pointer.Z];
    
    private static Signal[] IDLE =>
        [new ()];
    
    public static Signal[] FETCH =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
        REG_COMMIT(Pointer.MDR, Pointer.IR),
        STATE_COMMIT(State.DECODE),
    ];
    
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
        { FlagMask.CZVN, Flag.CARRY | Flag.ZERO | Flag.OVERFLOW | Flag.NEGATIVE },
        { FlagMask.ZVN, Flag.ZERO | Flag.OVERFLOW | Flag.NEGATIVE },
        { FlagMask.CZN, Flag.CARRY | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.ZN, Flag.ZERO | Flag.NEGATIVE },
    };
}

public enum FlagMask
{
    CZVN, ZVN, CZN, ZN,
}
