namespace Models.Pdp1170.Cpu.Microcodes;

public static partial class Microcode
{
    private static byte[] encodedModes;
    private static byte[] encodedRegs;
    private static Width encodedWidth;
    
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static Signal[] IDLE =>
        [new ()];
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_WRITE(Pointer.MDR, Pointer.IR),
    ];
    
    private static Signal STATE_COMMIT(State state) => new()
        { Cycle = Cycle.STATE_COMMIT, State = state, };
    
    private static Signal REG_WRITE(Pointer source, Pointer destination) => new()
        { Cycle = Cycle.REG_WRITE, First = source, Second = destination, Width = Width.WORD };
    private static Signal REG_WRITE(Pointer source, Pointer destination, Width width) => new()
        { Cycle = Cycle.REG_WRITE, First = source, Second = destination, Width = width };

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
