namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static ushort opcode;
    private static byte fzzz;
    private static byte zfzz;

    private const ushort colCount = 4096;
    
    public static Signal[][] Generate()
    {
        Signal[][] table = new Signal[256*256][];
        table[0] = [new Signal()];
        byte[] twoOprRows = [1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14];
        foreach (byte row in twoOprRows)
        {
            for (ushort col = 0; col < colCount; col++)
            {
                opcode = (ushort)((row << 12) | col);
                fzzz = (byte)(opcode >> 12);
                table[opcode] = TwoOperand();
                table[opcode][0].Name
                    = $"{TwoOperandNames[fzzz]} {AddressingModeNames(ooxxoo())},{AddressingModeNames(ooooxx())}";
            };
        }
        return table;
    }

    // OP SRC,DST
    
    private static Signal[] TwoOperand() => TWO_OPERAND(new Descriptor
    {
        Modes = [ooxooo(), ooooxo()],
        Regs = [EncodedRegisters[oooxoo()], EncodedRegisters[ooooox()]],
        Operation = TwoOperandTable[fzzz],
        Mask = fzzz is not (0x0 or 0x9) ? FlagMasks[FlagMask.CVZN] : FlagMasks[FlagMask.VZN],
        Writeback = fzzz is not (0x2 or 0x3 or 0xA or 0xB),
        Width = fzzz is >= 0xA and <= 0xD ? Width.BYTE : Width.WORD,
    });
    
    private static Signal[] OneOperand()
    {
        Descriptor descriptor = new()
        {
            Modes = [ooooxo()],
            Regs = [EncodedRegisters[ooooox()]],
            
            Operation = ((opcode >> 6) & 0x3F)  switch
            {
                0x3 => Operation.SWAB,
                0x30 => Operation.ROR,
                0x31 => Operation.ROL,
                0x32 => Operation.ASR,
                0x33 => Operation.ASL,
                0x37 => Operation.SXT,
                _ => SingleOperandTable[(opcode >> 6) & 0x7],
            },
        };

        descriptor.Mask = descriptor.Operation switch
        {
            Operation.INC or Operation.DEC => FlagMasks[FlagMask.VZN],
            Operation.PASS or Operation.SWAB => FlagMasks[FlagMask.ZN],
            Operation.SXT => Flag.ZERO,
            _ => FlagMasks[FlagMask.CVZN],
        };

        return [];
    }
    
    private static readonly Operation[] TwoOperandTable =
    [
        Operation.NONE, Operation.PASS, Operation.SUB, Operation.BIT, Operation.BIC,
        Operation.BIS, Operation.ADD, Operation.NONE, 
        Operation.NONE, Operation.PASS, Operation.SUB,
        Operation.BIT, Operation.BIC, Operation.BIS, Operation.SUB
    ];

    private static readonly string[] TwoOperandNames =
    [
        "", "MOV", "CMP", "BIT", "BIC", "BIS", "ADD",
        "", "", "MOVB", "CMPB", "BITB", "BICB", "BISB", "SUB",
    ];

    private static string AddressingModeNames(byte nibble)
    {
        string register = EncodedRegisters[nibble & 0x7].ToString();
        return (byte)(nibble >> 3) switch
        {
            0 => register, 1 => $"({register})",
            2 => $"({register})+", 3 => $"@({register})+",
            4 => $"-({register})", 5 => $"@-({register})",
            6 => $"X({register})", 7 => $"@X({register})",
        };
    }
    
    private static readonly Operation[] SingleOperandTable =
    [
        Operation.ZERO, Operation.COM, Operation.INC, Operation.DEC,
        Operation.NEG, Operation.ADC, Operation.SBC, Operation.PASS,
    ];
    
    public struct Descriptor()
    {
        public byte[] Modes;
        public Pointer[] Regs;
        public Operation Operation;
        public Flag Mask;
        public bool Writeback;
        public Width Width;
    }
    
    private static byte ooxooo()
        => (byte)((opcode >> 9) & 7);
    private static byte oooxoo()
        => (byte)((opcode >> 6) & 7);
    private static byte ooooxo()
        => (byte)((opcode >> 3) & 7);
    private static byte ooooox()
        => (byte)(opcode & 7);
    private static byte ooooxx()
        => (byte)(opcode & 0x3F);
    private static byte ooxxoo()
        => (byte)((opcode >> 6) & 0x3F);
}