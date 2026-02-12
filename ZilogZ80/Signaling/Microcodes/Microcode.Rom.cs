namespace ZilogZ80.Signaling.Microcodes;

public static partial class Microcode
{
    private static byte aa_XXa_aaa = 0;
    private static byte aa_XXX_aaa = 0;
    private static byte aa_aaa_XXX = 0;

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
}
