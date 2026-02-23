namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal STATE_COMMIT(State state) => new()
        { Cycle = Cycle.STATE_COMMIT, State = state, };
    
    private static Signal REG_MOVE(Pointer source, Pointer destination) => new()
        { Cycle = Cycle.REG_MOVE, First = source, Second = destination };

    private static Signal MEM_READ(Pointer address) => new()
        { Cycle = Cycle.MEM_READ, First = address, Width = Width.WORD };
    private static Signal MEM_READ(Pointer address, Width width) => new()
        { Cycle = Cycle.MEM_READ, First = address, Width = width };
    private static Signal MEM_WRITE(Pointer address) => new()
        { Cycle = Cycle.MEM_WRITE, First = address, Width = Width.WORD };
    private static Signal MEM_WRITE(Pointer address, Width width) => new()
        { Cycle = Cycle.MEM_WRITE, First = address, Width =  width };
    
    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, First = source, Second = operand, Mask = mask, Width = Width.WORD};
    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask, Width width) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, First = source, Second = operand, Mask = mask, Width =  width };
    
    private static Signal COND_COMPUTE(Condition condition, State state) => new()
        { Cycle = Cycle.COND_COMPUTE, Condition = condition, State = state };
    
    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
        { FlagMask.NONE, Flag.NONE },
        { FlagMask.CVZN, Flag.CARRY | Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.VZN, Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.VZ, Flag.OVERFLOW | Flag.ZERO },
        { FlagMask.ZN, Flag.ZERO | Flag.NEGATIVE },
    };
}
public enum FlagMask
{
    NONE, CVZN, VZN, VZ, ZN,
}