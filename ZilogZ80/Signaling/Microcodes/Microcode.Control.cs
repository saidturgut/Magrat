namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    // ------------------------- PUSH & POP ------------------------- //

    private static Signal[] PUSH(bool pc) =>
    [
        PAIR_DEC(SP),
        REG_COMMIT(pc ? Pointer.PCH : EncodedPairs[aa_XXa_aaa][1], Pointer.MDR),
        MEM_WRITE(SP),
        PAIR_DEC(SP),
        REG_COMMIT(pc ? Pointer.PCL : EncodedPairs[aa_XXa_aaa][0], Pointer.MDR),
        MEM_WRITE(SP),
    ];

    private static Signal[] POP(bool pc) =>
    [
        MEM_READ(SP),
        REG_COMMIT(pc ? Pointer.PCL : EncodedPairs[aa_XXa_aaa][0], Pointer.MDR),
        PAIR_INC(SP),
        MEM_READ(SP),
        REG_COMMIT(pc ? Pointer.PCH : EncodedPairs[aa_XXa_aaa][1], Pointer.MDR),
        PAIR_INC(SP),
    ];
    
    // ------------------------- CONTROL FLOW ------------------------- //

    private static Signal[] JMP(bool cond) =>
    [
        ..LOAD_PAIR_IMM(WZ),
        ..cond ? [REG_COMMIT(Pointer.F, Pointer.TMP), COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..JUMP_TO_PAIR(WZ),
    ];

    private static Signal[] CALL(bool cond) =>
    [
        ..LOAD_PAIR_IMM(WZ),
        ..cond ? [REG_COMMIT(Pointer.F, Pointer.TMP), COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..PUSH(true),
        ..JUMP_TO_PAIR(WZ),
    ];

    private static Signal[] RST =>
    [
        ..PUSH(true),
        ALU_COMPUTE(Operation.RST, Pointer.IR, Pointer.NIL, Flag.NONE),
        ..JUMP_TO_PAIR(TMP),
    ];

    private static Signal[] RET(bool cond) =>
    [
        ..cond ? [REG_COMMIT(Pointer.F, Pointer.TMP), COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..POP(true),
    ];
    
    private static Signal[] JMP_HL =>
    [
        MEM_READ(EncodedPairs[2]),
        REG_COMMIT(PointL, Pointer.SPL),
        REG_COMMIT(PointH, Pointer.SPH),
    ];
    
    // ------------------------- BRANCHING ------------------------- //
    
    private static Signal[] BRANCH(Condition condition) =>
    [
        ..READ_IMM,
        ..condition is not Condition.NONE ? [REG_COMMIT(Pointer.F, Pointer.TMP), COND_COMPUTE(condition)] : NONE,
        ..JUMP_INDEXED,
    ];
    
    private static Signal[] BRANCH_DJ =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.F, Pointer.W),
        ALU_COMPUTE(Operation.DEC, Pointer.B, Pointer.NIL, Flag.ZERO),
        REG_COMMIT(Pointer.TMP, Pointer.B),
        REG_COMMIT(Pointer.F, Pointer.TMP),
        REG_COMMIT(Pointer.W, Pointer.F),
        COND_COMPUTE(Condition.NZ),
        ..JUMP_INDEXED,
    ];
}