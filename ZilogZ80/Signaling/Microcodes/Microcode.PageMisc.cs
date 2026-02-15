namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> MiscPage = new()
    {
        // 16 BIT ARITHMETIC
        ["ADD_WORD"] = () => ALU_WORD(Operation.ADD, Operation.ADC, FlagMask.CNV3H5ZS), 
        ["SUB_WORD"] = () => ALU_WORD(Operation.SBC, Operation.SBC, FlagMask.CNV3H5ZS),
        ["PAIR_TO_ABS"] = () => PAIR_TO_ABS(EncodedPairs![aa_XXa_aaa]), ["ABS_TO_PAIR"] = () => ABS_TO_PAIR(EncodedPairs![aa_XXa_aaa]),
        
        // INPUT & OUTPUT
        ["INPUT_BC"] = () => INPUT_OUTPUT_BC(true), ["OUTPUT_BC"] = () => INPUT_OUTPUT_BC(false),
        ["INPUT_BLK_INC"] = () => INPUT_OUTPUT_BLK(true, true, false), ["OUTPUT_BLK_INC"] = () => INPUT_OUTPUT_BLK(false, true, false),
        ["INPUT_BLK_DEC"] = () => INPUT_OUTPUT_BLK(true, false, false), ["OUTPUT_BLK_DEC"] = () => INPUT_OUTPUT_BLK(false, false, false),
        ["INPUT_REP_INC"] = () => INPUT_OUTPUT_BLK(true, true, true), ["OUTPUT_REP_INC"] = () => INPUT_OUTPUT_BLK(false, true, true),
        ["INPUT_REP_DEC"] = () => INPUT_OUTPUT_BLK(true, false, true), ["OUTPUT_REP_DEC"] = () => INPUT_OUTPUT_BLK(false, false, true),
        
        // TRANSFER & COMPARE
        ["COPY_INC"] = () => TRANSFER(true, false), ["COMP_INC"] = () => COMPARE(true, false),
        ["COPY_DEC"] = () => TRANSFER(false, false), ["COMP_DEC"] = () => COMPARE(false, false),
        ["COPY_REP_INC"] = () => TRANSFER(true, true), ["COMP_REP_INC"] = () => COMPARE(true, true),
        ["COPY_REP_DEC"] = () => TRANSFER(false, true), ["COMP_REP_DEC"] = () => COMPARE(false, true),
    };
    
    private static Signal[] TRANSFER(bool inc, bool loop) =>
    [
        MEM_READ(HL),
        MEM_WRITE(DE),
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        inc ? PAIR_INC(DE) : PAIR_DEC(DE),
        PAIR_DEC(BC),
        ALU_COMPUTE(Operation.BLK, Pointer.B, Pointer.C, FlagMasks[FlagMask.NV3H5]),
        REG_COMMIT(Pointer.F, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.PE)] : NONE,
    ];
    
    private static Signal[] COMPARE(bool inc, bool loop) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(Operation.SUB, Pointer.A, Pointer.MDR, FlagMasks[FlagMask.N3H5ZS]),
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        PAIR_DEC(BC),
        ALU_COMPUTE(Operation.BLK, Pointer.B, Pointer.C, Flag.OVER),
        REG_COMMIT(Pointer.F, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.ED)] : NONE,
    ];

    private static Signal[] INPUT_OUTPUT_BC(bool input) =>
    [
        ..input
            ? (Signal[])
            [
                MEM_READ(BC),
                ALU_COMPUTE(Operation.IOP, Pointer.MDR, Pointer.NIL, FlagMasks[FlagMask.NVHZS]),
                REG_COMMIT(Pointer.MDR, EncodedRegisters[aa_XXX_aaa]),
            ]
            : aa_XXX_aaa != 6 
                ? new[] { REG_COMMIT(EncodedRegisters[aa_XXX_aaa], Pointer.MDR), MEM_WRITE(BC), } 
                : new[] { REG_COMMIT(Pointer.NIL, Pointer.MDR), MEM_WRITE(BC), }
    ];

    private static Signal[] INPUT_OUTPUT_BLK(bool input, bool inc, bool loop) =>
    [
        ..input
            ? (Signal[])[MEM_READ(BC), MEM_WRITE(HL)] 
            : (Signal[])[MEM_READ(HL), MEM_WRITE(BC)],
        
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        ALU_COMPUTE(Operation.DEC, Pointer.B, Pointer.NIL, FlagMasks[FlagMask.NZ]),
        REG_COMMIT(Pointer.TMP, Pointer.B),
        REG_COMMIT(Pointer.F, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.Z)] : NONE,
    ];
}