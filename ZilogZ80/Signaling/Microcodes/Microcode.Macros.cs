namespace ZilogZ80.Signaling.Microcodes;

public partial class Microcode
{
    // ------------------------- MACROS ------------------------- //

    private static Signal[] READ_IMM =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
    ];
    
    private static Signal[] LOAD_PAIR_IMM(Pointer[] pair) =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, pair[0]),
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, pair[1]),
    ];
    
    private static Signal[] JUMP_TO_PAIR(Pointer[] pair) =>
    [
        REG_COMMIT(pair[0], Pointer.PCL),
        REG_COMMIT(pair[1], Pointer.PCH),
    ];
}