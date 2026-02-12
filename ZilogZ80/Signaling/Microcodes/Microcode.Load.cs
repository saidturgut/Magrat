namespace ZilogZ80.Signaling.Microcodes;

public partial class Microcode
{
    // ------------------------- 8 BIT OPERATIONS ------------------------- //

    private static Signal[] REG_TO_REG =>
    [
        REG_COMMIT(EncodedRegisters[aa_aaa_XXX], EncodedRegisters[aa_XXX_aaa])
    ];
    
    private static Signal[] REG_TO_MEM(bool imm) => // IMM_TO_MEM
    [
        ..imm ? READ_IMM : [REG_COMMIT(EncodedRegisters[aa_XXX_aaa], Pointer.MDR)],
        MEM_WRITE(HL),
    ];
    private static Signal[] MEM_TO_REG(bool imm) => // IMM_TO_REG
    [
        ..imm ? READ_IMM : [MEM_READ(HL)],
        REG_COMMIT(Pointer.MDR, EncodedRegisters[aa_XXX_aaa])
    ];
    
    private static Signal[] ACC_TO_MEM =>
    [
        REG_COMMIT(Pointer.A, Pointer.MDR),
        MEM_WRITE(EncodedPairs[aa_XXa_aaa]),
    ];
    private static Signal[] MEM_TO_ACC =>
    [
        MEM_READ(EncodedPairs[aa_XXa_aaa]),
        REG_COMMIT(Pointer.MDR, Pointer.A),
    ];
    
    // ------------------------- 16 BIT OPERATIONS ------------------------- //
    
    private static Signal[] IMM_TO_PAIR =>
    [
        ..LOAD_PAIR_IMM(EncodedPairs[aa_XXa_aaa]),
    ];
    private static Signal[] HL_TO_SP =>
    [
        REG_COMMIT(Pointer.L, Pointer.SPL),
        REG_COMMIT(Pointer.H, Pointer.SPH),
    ];
    
    private static Signal[] ACC_TO_ABS =>
    [
        ..LOAD_PAIR_IMM(WZ),
        REG_COMMIT(Pointer.A, Pointer.MDR),
        MEM_WRITE(WZ),
    ];
    private static Signal[] ABS_TO_ACC =>
    [
        ..LOAD_PAIR_IMM(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.A),
    ];
    
    private static Signal[] HL_TO_ABS =>
    [
        ..LOAD_PAIR_IMM(WZ),
        REG_COMMIT(Pointer.L, Pointer.MDR),
        MEM_WRITE(WZ),
        PAIR_INC(WZ),
        REG_COMMIT(Pointer.H, Pointer.MDR),
        MEM_WRITE(WZ),
    ];
    private static Signal[] ABS_TO_HL =>
    [
        ..LOAD_PAIR_IMM(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.L),
        PAIR_INC(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.H),
    ];
}