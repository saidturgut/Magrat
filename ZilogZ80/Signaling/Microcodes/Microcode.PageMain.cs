namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> MainPage = new()
    {
        // ------------------------- BASIC INSTRUCTIONS ------------------------- //

        // STATE OPS
        ["-"] = () => [STATE_COMMIT(Cycle.IDLE)], 
        ["NOP"] = () => IDLE, ["HALT"] = () => [STATE_COMMIT(Cycle.HALT)],
        ["INT_ON"] = () => IDLE, ["INT_OFF"] = () => IDLE,
        ["INPUT"] = () => INPUT_OUTPUT(true), ["OUTPUT"] = () => INPUT_OUTPUT(false),

        // ------------------------- LOAD INSTRUCTIONS ------------------------- //
        
        // 8 BIT LOAD
        ["REG_TO_REG"] = () => REG_TO_REG,
        ["REG_TO_MEM"] = () => REG_TO_MEM(false), ["MEM_TO_REG"] = () => MEM_TO_REG(false),
        ["IMM_TO_MEM"] = () => REG_TO_MEM(true), ["IMM_TO_REG"] = () => MEM_TO_REG(true),
        ["ACC_TO_MEM"] = () => ACC_TO_MEM, ["MEM_TO_ACC"] = () => MEM_TO_ACC,
        
        // 16 BIT LOAD
        ["IMM_TO_PAIR"] = () => IMM_TO_PAIR, ["HL_TO_SP"] = () => HL_TO_SP,
        ["ACC_TO_ABS"] = () => ACC_TO_ABS, ["ABS_TO_ACC"] = () => ABS_TO_ACC,
        ["HL_TO_ABS"] = () => HL_TO_ABS, ["ABS_TO_HL"] = () => ABS_TO_HL,
        
        // ------------------------- ALU INSTRUCTIONS ------------------------- //
        
        // ARITHMETIC & LOGIC
        ["ALU_REG"] = () => ALU_REG(EncodedAluOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true), 
        ["ALU_MEM"] = () => ALU_MEM(true), ["ALU_IMM"] = () => ALU_IMM(true),
        ["CMP_REG"] = () => ALU_REG(EncodedAluOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, false), 
        ["CMP_MEM"] = () => ALU_MEM(false), ["CMP_IMM"] = () => ALU_IMM(false),
        ["ADD_WORD"] = () => ADD_WORD, 
        
        // INCREMENT & DECREMENT
        ["INC_REG"] = () => INC_REG(Operation.INC), ["INC_MEM"] = () => INC_MEM(Operation.INC),
        ["DEC_REG"] = () => INC_REG(Operation.DEC), ["DEC_MEM"] = () => INC_MEM(Operation.DEC),
        ["INC_WORD"] = () => INC_WORD(true), ["DEC_WORD"] = () => INC_WORD(false),
        
        // SHIFT & ROTATE
        ["ALU_SHIFT"] = () => ALU_REG(EncodedBitOperations![aa_XXX_aaa], FlagMask.CN3H5, true),
        ["DAA"] = () => ALU_REG(Operation.DAA, FlagMask.CV3H5ZS, true), 

        // FLAG OPS
        ["SCF"] = () => ALU_FLAG(Operation.SCF), ["CCF"] = () => ALU_FLAG(Operation.CCF),
        ["CPL"] = () => ALU_REG(Operation.CPL, FlagMask.N3H5, true),
        
        // ------------------------- CONTROL INSTRUCTIONS ------------------------- //
        
        // CONTROL FLOW
        ["PUSH"] = () => PUSH(false), ["POP"] = () => POP(false),
        ["JMP"] = () => JMP(false), ["JMP_CN"] = () => JMP(true),
        ["CALL"] = () => CALL(false), ["CALL_CN"] = () => CALL(true),
        ["RET"] = () => RET(false), ["RET_CN"] = () => RET(true),
        ["RST"] = () => RST, ["JMP_HL"] = () => JMP_HL,
        
        // BRANCH OPS
        ["BRANCH"] = () => BRANCH(Condition.NONE), ["BRANCH_DJ"] = () => BRANCH_DJ,
        ["BRANCH_CN"] = () => BRANCH((Condition)(aa_XXX_aaa & 0b011)),
        
        // ------------------------- SWAP INSTRUCTIONS ------------------------- //
        
        // SWAP OPS
        ["SWAP_ALL"] = () => SWAP_ALL, ["SWAP_XTHL"] = () => SWAP_XTHL,
        ["SWAP_AF"] = () => SWAP_AF, ["SWAP_XCHG"] = () => SWAP_XCHG,
        
        // ------------------------- DISPLACEMENT OPS ------------------------- //

        ["REG_TO_MEM_D"] = () => [..IDX, ..REG_TO_MEM(false)],
        ["MEM_TO_REG_D"] = () => [..IDX, ..MEM_TO_REG(false)],
        ["IMM_TO_MEM_D"] = () => [..IDX, ..REG_TO_MEM(true)],
        ["ALU_MEM_D"] = () => [..IDX, ..ALU_MEM(true)], 
        ["CMP_MEM_D"] = () => [..IDX, ..ALU_MEM(false)],
        ["INC_MEM_D"] = () => [..IDX, ..INC_MEM(Operation.INC)], 
        ["DEC_MEM_D"] = () => [..IDX, ..INC_MEM(Operation.DEC)],
    };
    
    private static readonly Pointer[] EncodedRegisters =
    [
        Pointer.B, Pointer.C, Pointer.D, Pointer.E, 
        Pointer.H, Pointer.L,
        Pointer.MDR, Pointer.A,
    ];
    private static readonly Pointer[][] EncodedPairs =
    [
        [Pointer.C, Pointer.B],
        [Pointer.E, Pointer.D],
        [Pointer.L, Pointer.H],
        [Pointer.SPL, Pointer.SPH],
    ];

    private static readonly Operation[] EncodedAluOperations =
    [
        Operation.ADD, Operation.ADC, 
        Operation.SUB, Operation.SBC, 
        Operation.ANA, Operation.XRA, 
        Operation.ORA, Operation.SUB,
    ];
}
