namespace ZilogZ80.Decoding;
using Signaling;
using Computing;
using Kernel;

public partial class Microcode
{
    // ------------------------- ARITHMETIC & LOGIC ------------------------- //

    private static Signal[] WRITE_BACK(bool commit) =>
        [..commit ? [REG_COMMIT(Pointer.TMP, Pointer.A)] : NONE,];
    
    private static Signal[] ALU_REG(Operation operation, FlagMask mask, bool commit) =>
    [
        ALU_COMPUTE(operation, Pointer.A, EncodedRegisters[aa_aaa_XXX], FlagMasks[mask]),
        ..WRITE_BACK(commit),
    ];
    private static Signal[] ALU_MEM(bool commit) =>
    [
        MEM_READ(addrSrc),
        ALU_COMPUTE(EncodedAluOperations[aa_XXX_aaa], Pointer.A, EncodedRegisters[aa_aaa_XXX], FlagMasks[FlagMask.CNV3H5ZS]),
        ..WRITE_BACK(commit),
    ];
    private static Signal[] ALU_IMM(bool commit) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(EncodedAluOperations[aa_XXX_aaa], Pointer.A, Pointer.MDR, FlagMasks[FlagMask.CNV3H5ZS]),
        ..WRITE_BACK(commit),
    ];
    
    private static Signal[] ALU_FLAG(Operation operation) =>
    [
        ALU_COMPUTE(operation, Pointer.NIL, Pointer.NIL, FlagMasks[FlagMask.CN3H5]),
    ];
    
    private static Signal[] ALU_WORD(Operation low, Operation high, FlagMask mask) =>
    [
        ALU_COMPUTE(low, PointL, EncodedPairs[aa_XXa_aaa][0], Flag.CARRY),
        REG_COMMIT(Pointer.TMP, PointL),
        ALU_COMPUTE(high, PointH, EncodedPairs[aa_XXa_aaa][1], FlagMasks[mask]),
        REG_COMMIT(Pointer.TMP, PointH),
    ];
    
    // ------------------------- INCREMENT & DECREMENT  ------------------------- //
    
    private static Signal[] INC_REG(Operation operation) =>
    [
        ALU_COMPUTE(operation, EncodedRegisters[aa_XXX_aaa], Pointer.NIL, FlagMasks[FlagMask.NV3H5ZS]),
        REG_COMMIT(Pointer.TMP, EncodedRegisters[aa_XXX_aaa]),
    ];
    private static Signal[] INC_MEM(Operation operation) =>
    [
        MEM_READ(addrSrc),
        ALU_COMPUTE(operation, Pointer.A, Pointer.MDR, FlagMasks[FlagMask.NV3H5ZS]),
        REG_COMMIT(Pointer.TMP, Pointer.MDR),
        MEM_WRITE(addrSrc)
    ];

    private static Signal[] INC_WORD(bool inx) =>
    [
        inx ? PAIR_INC(EncodedPairs[aa_XXa_aaa]) : PAIR_DEC(EncodedPairs[aa_XXa_aaa]),
    ];
    
    // ------------------------- CB PAGE OPS  ------------------------- //
    
    private static Signal[] CB_REG(Operation operation, FlagMask mask, bool commit) =>
    [
        ALU_COMPUTE(operation, EncodedRegisters[aa_aaa_XXX], Pointer.IR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, EncodedRegisters[aa_aaa_XXX])] : NONE,
    ];
    private static Signal[] CB_MEM(Operation operation, FlagMask mask, bool commit) =>
    [
        MEM_READ(addrSrc),
        ALU_COMPUTE(operation, Pointer.MDR, Pointer.IR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.MDR), MEM_WRITE(addrSrc)] : NONE,
    ];
}