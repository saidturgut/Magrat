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
        ..cond ? [COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..LOAD_PAIR_IMM(WZ),
        ..JUMP_TO_PAIR(WZ),
    ];

    private static Signal[] CALL(bool cond) =>
    [
        ..cond ? [COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..LOAD_PAIR_IMM(WZ),
        ..PUSH(true),
        ..JUMP_TO_PAIR(WZ),
    ];

    private static Signal[] RST =>
    [
        ..PUSH(true),
        ALU_COMPUTE(Operation.RST, Pointer.IR, Pointer.NIL, Flag.NONE),
        ..JUMP_TO_PAIR([Pointer.TMP, Pointer.NIL]),
    ];

    private static Signal[] RET(bool cond) =>
    [
        ..cond ? [COND_COMPUTE((Condition)aa_XXX_aaa)] : NONE,
        ..POP(true),
    ];
}