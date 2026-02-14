namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> IdxPage = new()
    {
        //IMM_TO_PAIR
        //HL_TO_ABS
        //INC_WORD
        //INC_REG
        //DEC_REG
        //IMM_TO_REG
        //ADD_WORD
        ["IMM_TO_MEM"] = () => REG_TO_MEM(true),
        ["REG_TO_MEM"] = () => REG_TO_MEM(false), 
        ["MEM_TO_REG"] = () => MEM_TO_REG(false),
        ["CMP_MEM"] = () => ALU_MEM(false),
        ["ALU_MEM"] = () => ALU_MEM(true),
        ["INC_MEM"] = () => INC_MEM(Operation.INC),
        ["DEC_MEM"] = () => INC_MEM(Operation.DEC),
    };
    
    private static Signal[][] PageIdx()
    {
        EncodedRegisters[4] = Pointer.IXL;
        EncodedRegisters[5] = Pointer.IXH;
        EncodedPairs[2] = [Pointer.IXL, Pointer.IXH];
        Signal[][] table = PageMain("PageIdx", IdxPage, false);
        
        return table;
    }
}