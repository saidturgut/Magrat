namespace ZilogZ80.Decoding;
using Signaling;
using Computing;
using Kernel;

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
    
    private static Signal[] JUMP_INDEXED =>
    [
        ALU_COMPUTE(Operation.IDX, Pointer.PCL, Pointer.MDR, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.PCL),
        ALU_COMPUTE(Operation.SXT, Pointer.PCH, Pointer.MDR, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.PCH),
    ];
    
    private static Signal[] DISPLACEMENT =>
    [
        ALU_COMPUTE(Operation.IDX, PointL, Pointer.MDR, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.W),
        ALU_COMPUTE(Operation.SXT, PointH, Pointer.MDR, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.Z),
    ];
    
    private static Signal[] REG_SWAP(Pointer first, Pointer second) =>
    [
        REG_COMMIT(first, Pointer.TMP),
        REG_COMMIT(second, first),
        REG_COMMIT(Pointer.TMP, second),
    ];
    
    private static Signal[] PAIR_SWAP(Pointer[] first, Pointer[] second) =>
    [
        ..REG_SWAP(first[0], second[0]),
        ..REG_SWAP(first[1], second[1]),
    ];
}