namespace ZilogZ80.Signaling.Microcodes;
using Executing.Computing;

public partial class Microcode
{
    private static Signal[] CB_REG(Operation operation, FlagMask mask, bool commit) =>
    [
        ALU_COMPUTE(operation, EncodedRegisters[aa_aaa_XXX], Pointer.IR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, EncodedRegisters[aa_aaa_XXX])] : NONE,
    ];
    private static Signal[] CB_MEM(Operation operation, FlagMask mask, bool commit) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(operation, Pointer.MDR, Pointer.IR, FlagMasks[mask]),
        ..commit ? [REG_COMMIT(Pointer.TMP, Pointer.MDR), MEM_WRITE(HL)] : NONE,
    ];
    
    public static Signal[][] PageBit(bool dump)
    {
        var table = new Signal[256][];
        
        for (byte i = 0; i < 4; i++)
        {
            for (byte j = 0; j < 8; j++)
            {
                for (byte k = 0; k < 8; k++)
                {
                    byte opcode = (byte)((i << 6) | (j << 3) | k);                    
                    SetNibbles(opcode);
                    string reg;
                    if (aa_aaa_XXX != 0x6)
                    {
                        reg = EncodedRegisters[aa_aaa_XXX].ToString();
                        table[opcode] = i switch
                        {
                            0 => CB_REG(EncodedBitOperations[aa_XXX_aaa], FlagMask.CNV3H5ZS, true),
                            1 => CB_REG(Operation.BIT, FlagMask.NV3H5ZS, false),
                            2 => CB_REG(Operation.RES, FlagMask.NONE, true),
                            3 => CB_REG(Operation.SET, FlagMask.NONE, true),
                        };
                    }
                    else
                    {
                        reg = "(HL)";
                        table[opcode] = i switch
                        {
                            0 => CB_MEM(EncodedBitOperations[aa_XXX_aaa], FlagMask.CNV3H5ZS, true),
                            1 => CB_MEM(Operation.BIT, FlagMask.NV3H5ZS, false),
                            2 => CB_MEM(Operation.RES, FlagMask.NONE, true),
                            3 => CB_MEM(Operation.SET, FlagMask.NONE, true),
                        };
                    }
                    Signal first = table[opcode][0];
                    table[opcode][0].Name = $"{first.Operation} " + (i == 0 ? reg : $"{aa_XXX_aaa},{reg}");
                }
            }
        }
        if(dump) Dump(table);
        return table;
    }
    private static readonly Operation[] EncodedBitOperations =
    [
        Operation.RLC, Operation.RRC, 
        Operation.RAL, Operation.RAR,
        Operation.SLA, Operation.SRA,
        Operation.SLL, Operation.SRL,
    ];
}