using ZilogZ80.Executing.Computing;

namespace ZilogZ80.Signaling.Microcodes;

public partial class Microcode
{
    private static Signal[] ALU_REG(Operation operation, FlagMask mask, bool commit) =>
    [
        ALU_COMPUTE(operation, Pointer.A, EncodedRegisters[aa_aaa_XXX], FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    private static Signal[] ALU_MEM(Operation operation, FlagMask mask, bool commit) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(operation, Pointer.A, Pointer.MDR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    private static Signal[] ALU_IMM(Operation operation, FlagMask mask, bool commit) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(operation, Pointer.A, Pointer.MDR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    
    private static Signal[] ALU_FLAG(Operation operation) =>
    [
        ALU_COMPUTE(operation, Pointer.NIL, Pointer.NIL, FlagMasks[FlagMask.CN3H5]),
    ];
    
    private static Signal[] INC_REG(Operation operation) =>
    [
        ALU_COMPUTE(operation, EncodedRegisters[aa_XXX_aaa], Pointer.NIL, FlagMasks[FlagMask.NV3H5ZS]),
        REG_COMMIT(Pointer.TMP, EncodedRegisters[aa_XXX_aaa]),
    ];
    private static Signal[] INC_MEM(Operation operation) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(operation, Pointer.A, Pointer.MDR, FlagMasks[FlagMask.NV3H5ZS]),
        REG_COMMIT(Pointer.TMP, Pointer.MDR),
        MEM_WRITE(HL)
    ];

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