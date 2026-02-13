namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static byte aa_XXa_aaa;
    private static byte aa_XXX_aaa;
    private static byte aa_aaa_XXX;

    private static readonly Dictionary<string, Func<Signal[]>> MainPage = new()
    {
        // BASIC
        ["-"] = () => [STATE_COMMIT(Cycle.IDLE)], 
        ["NOP"] = () => [STATE_COMMIT(Cycle.IDLE)], ["HALT"] = () => [STATE_COMMIT(Cycle.HALT)],
        
        // 8 BIT LOAD
        ["REG_TO_REG"] = () => REG_TO_REG,
        ["REG_TO_MEM"] = () => REG_TO_MEM(false), ["MEM_TO_REG"] = () => MEM_TO_REG(false),
        ["IMM_TO_MEM"] = () => REG_TO_MEM(true), ["IMM_TO_REG"] = () => MEM_TO_REG(true),
        ["ACC_TO_MEM"] = () => ACC_TO_MEM, ["MEM_TO_ACC"] = () => MEM_TO_ACC,
        
        // 16 BIT LOAD
        ["IMM_TO_PAIR"] = () => IMM_TO_PAIR, ["HL_TO_SP"] = () => HL_TO_SP,
        ["ACC_TO_ABS"] = () => ACC_TO_ABS, ["ABS_TO_ACC"] = () => ABS_TO_ACC,
        ["HL_TO_ABS"] = () => HL_TO_ABS, ["ABS_TO_HL"] = () => ABS_TO_HL,
        
        // ALU
        ["ALU_REG"] = () => ALU_REG(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true), ["ALU_MEM"] = () => ALU_MEM(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true), 
        ["ALU_IMM"] = () => ALU_IMM(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true),
        ["CMP_REG"] = () => ALU_REG(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, false), ["CMP_MEM"] = () => ALU_MEM(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, false), 
        ["CMP_IMM"] = () => ALU_IMM(EncodedOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, false),
        ["INC_REG"] = () => INC_REG(Operation.INC), ["INC_MEM"] = () => INC_MEM(Operation.INC),
        ["DEC_REG"] = () => INC_REG(Operation.DEC), ["DEC_MEM"] = () => INC_MEM(Operation.DEC),

        // BIT
        ["SCF"] = () => ALU_FLAG(Operation.SCF), ["CCF"] = () => ALU_FLAG(Operation.CCF),
        ["DAA"] = () => ALU_REG(Operation.DAA, FlagMask.CV3H5ZS, true), ["CPL"] = () => ALU_REG(Operation.CPL, FlagMask.N3H5, true),
        ["ALU_SHIFT"] = () => ALU_REG(EncodedShiftOperations![aa_XXX_aaa], FlagMask.CN3H5, true),
    };
    
    private static readonly Pointer[] EncodedRegisters =
    [
        Pointer.B, Pointer.C, Pointer.D, Pointer.E, 
        Pointer.H, Pointer.L,
        Pointer.NIL, Pointer.A,
    ];
    private static readonly Pointer[][] EncodedPairs =
    [
        [Pointer.C, Pointer.B],
        [Pointer.E, Pointer.D],
        [Pointer.L, Pointer.H],
        [Pointer.SPL, Pointer.SPH],
    ];

    private static readonly Operation[] EncodedOperations =
    [
        Operation.ADD, Operation.ADC, 
        Operation.SUB, Operation.SBC, 
        Operation.ANA, Operation.XRA, 
        Operation.ORA, Operation.SUB,
    ];
    
    private static readonly Operation[] EncodedShiftOperations =
    [
        Operation.RLC, Operation.RRC, 
        Operation.RAL, Operation.RAR, 
    ];
}
