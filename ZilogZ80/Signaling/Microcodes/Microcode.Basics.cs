namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static Signal[] IDLE =>
        [new ()];
    
    public static Signal[] FETCH =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.IR),
        ALU_COMPUTE(Operation.RFR, Pointer.R, Pointer.NIL, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.R),
        STATE_COMMIT(State.DECODE),
    ];
    
    public static Signal[] PREFIX =>
    [
        REG_COMMIT(Pointer.IR, Pointer.W),
        ..FETCH,
    ];

    private static Signal[] IDX =>
    [
        ..READ_IMM,
        ..DISPLACEMENT,
    ];

    private static Signal[] IDX_BIT =>
    [
        REG_COMMIT(Pointer.W, Pointer.MDR),
        REG_COMMIT(Pointer.NIL, Pointer.W),
        ..DISPLACEMENT,
    ];

    public static Signal[] INT_1 =>
    [
        ..PUSH(true),
        ALU_COMPUTE(Operation.VEC, Pointer.NIL, Pointer.NIL, Flag.NONE),
        ..JUMP_TO_PAIR([Pointer.TMP, Pointer.NIL]),
    ];

    private static Signal[] INT_CONTROL(State state, bool pop) =>
    [
        ..pop ? POP(true) : NONE,
        STATE_COMMIT(state),
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