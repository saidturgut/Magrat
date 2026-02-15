namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> BitPage = new()
    {
        ["-"] = () => IDLE,
        
        // ------------------------- DEF BIT PAGE ------------------------- //

         ["ROT_REG"] = () => CB_REG(EncodedBitOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true),
         ["ROT_MEM"] = () => CB_MEM(EncodedBitOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true),
         ["BIT_REG"] = () => CB_REG(Operation.BIT, FlagMask.NV3H5ZS, false),
         ["BIT_MEM"] = () => CB_MEM(Operation.BIT, FlagMask.NV3H5ZS, false),
         ["RES_REG"] = () => CB_REG(Operation.RES, FlagMask.NONE, true),
         ["RES_MEM"] = () => CB_MEM(Operation.RES, FlagMask.NONE, true),
         ["SET_REG"] = () => CB_REG(Operation.SET, FlagMask.NONE, true),
         ["SET_MEM"] = () => CB_MEM(Operation.SET, FlagMask.NONE, true),
         
         // ------------------------- IDX BIT PAGE ------------------------- //
         
         ["ROT_MEM_D"] = () => [..IDX_BIT, ..CB_MEM(EncodedBitOperations![aa_XXX_aaa], FlagMask.CNV3H5ZS, true)],
         ["BIT_MEM_D"] = () => [..IDX_BIT, ..CB_MEM(Operation.BIT, FlagMask.NV3H5ZS, false)],
         ["RES_MEM_D"] = () => [..IDX_BIT, ..CB_MEM(Operation.RES, FlagMask.NONE, true)],
         ["SET_MEM_D"] = () => [..IDX_BIT, ..CB_MEM(Operation.SET, FlagMask.NONE, true)],
    };

    private static readonly Operation[] EncodedBitOperations =
    [
        Operation.RLC, Operation.RRC, 
        Operation.RAL, Operation.RAR,
        Operation.SLA, Operation.SRA,
        Operation.SLL, Operation.SRL,
    ];
}