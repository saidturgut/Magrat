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
}