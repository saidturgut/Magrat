namespace Models.Pdp1170.Cpu.Microcodes;

public static partial class Microcode
{
    private static readonly Signal[] NONE = Array.Empty<Signal>();
    
    private static Signal[] IDLE =>
        [new ()];
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_WRITE(Pointer.MDR, Pointer.IR),
        STATE_COMMIT(State.DECODE),
    ];
}

