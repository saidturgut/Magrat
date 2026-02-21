namespace Models.x8Bit.ZilogZ80.Decoding;
using Signaling;
using Computing;
using Models.x8Bit.Engine;

public partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> MiscPage = new()
    {
        ["-"] = () => IDLE, 
        
        // INTERRUPTS
        ["INT_0"] = () => INT_CONTROL(State.INT_0, false), ["INT_1"] = () => INT_CONTROL(State.INT_1, false), 
        ["INT_2"] = () => INT_CONTROL(State.INT_2, false), ["INT_R"] = () => INT_CONTROL(State.INT_R, true),
        ["INT_AI"] = () => INT_AA(Pointer.I), ["INT_AR"] = () => INT_AA(Pointer.R),
        ["INT_IA"] = () => INT_RI(Pointer.I), ["INT_RA"] = () => INT_RI(Pointer.R),
        ["INT_I"] = () => POP(true), 

        // 8 BIT ARITHMETIC
        ["DECIMAL_ROT_R"] = () => DECIMAL_ROT(Operation.RRL, Operation.RRH),
        ["DECIMAL_ROT_L"] = () => DECIMAL_ROT(Operation.RLL, Operation.RLH),
        ["NEG"] = () => NEG,
        
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
}