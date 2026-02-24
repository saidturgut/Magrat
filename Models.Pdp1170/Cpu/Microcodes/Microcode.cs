namespace Models.Pdp1170.Cpu.Microcodes;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_MOVE(Pointer.MDR, Pointer.IR),
        STATE_COMMIT(State.DECODE),
    ];
    
    private static Signal[] SET_NAME(string nam)
    {
        name = nam; 
        return NONE;
    }
    
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
    
    private static Signal ALU_COMPUTE(Operation operation, Flag mask) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, Mask = mask, Width = Width.WORD};
    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, First = source, Second = operand, Mask = mask, Width = Width.WORD};
    private static Signal ALU_COMPUTE(Operation operation, Pointer source, Pointer operand, Flag mask, Width width) => new()
        { Cycle = Cycle.ALU_COMPUTE, Operation = operation, First = source, Second = operand, Mask = mask, Width =  width };
    
    private static Signal COND_COMPUTE(Condition condition, State state) => new()
        { Cycle = Cycle.COND_COMPUTE, Condition = condition, State = state };
}

