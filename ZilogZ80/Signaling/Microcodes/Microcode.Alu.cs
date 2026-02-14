namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    // ------------------------- ARITHMETIC & LOGIC ------------------------- //

    private static Signal[] ALU_REG(Operation operation, FlagMask mask, bool commit) =>
    [
        ALU_COMPUTE(operation, Pointer.A, EncodedRegisters[aa_aaa_XXX], FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    private static Signal[] ALU_MEM(bool commit) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(EncodedOperations[aa_XXX_aaa], Pointer.A, Pointer.MDR, FlagMasks[FlagMask.CNV3H5ZS]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    private static Signal[] ALU_IMM(bool commit) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(EncodedOperations[aa_XXX_aaa], Pointer.A, Pointer.MDR, FlagMasks[FlagMask.CNV3H5ZS]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,
    ];
    
    private static Signal[] ALU_FLAG(Operation operation) =>
    [
        ALU_COMPUTE(operation, Pointer.NIL, Pointer.NIL, FlagMasks[FlagMask.CN3H5]),
    ];
    
    private static Signal[] ADD_WORD =>
    [
        ALU_COMPUTE(Operation.ADD, Pointer.L, EncodedPairs[aa_XXa_aaa][0], Flag.CARRY),
        REG_COMMIT(Pointer.TMP, Pointer.L),
        ALU_COMPUTE(Operation.ADC, Pointer.H, EncodedPairs[aa_XXa_aaa][1], FlagMasks[FlagMask.CNH]),
        REG_COMMIT(Pointer.TMP, Pointer.H),
    ];
    
    // ------------------------- INCREMENT & DECREMENT  ------------------------- //
    
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

    private static Signal[] INC_WORD(bool inx) =>
    [
        inx ? PAIR_INC(EncodedPairs[aa_XXa_aaa]) : PAIR_DEC(EncodedPairs[aa_XXa_aaa]),
    ];
}