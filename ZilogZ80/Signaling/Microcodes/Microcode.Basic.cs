namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
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

    private static Signal[] INPUT_OUTPUT(bool input) =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
        ALU_COMPUTE(Operation.TOP, Pointer.NIL, Pointer.NIL, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.Z),
        ..input 
            ? (Signal[])[MEM_READ(WZ), REG_COMMIT(Pointer.MDR, Pointer.A),]
            : [REG_COMMIT(Pointer.A, Pointer.MDR), MEM_WRITE(WZ)],
    ];
}