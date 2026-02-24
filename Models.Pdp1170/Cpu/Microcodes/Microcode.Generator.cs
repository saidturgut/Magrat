using System.Data;

namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static ushort opcode;
    private static string name = "";
    private static string cell = "";
    
    public static Signal[][] Generate()
    {
        Signal[][] table = new Signal[256*256][];
        string[] lines = File.ReadAllLines("Pdp11Matrix.csv");
        for (int row = 0; row < 16; row++)
        {
            string[] cells = lines[row].Split(',');
            for (int col = 0; col < 4096; col++)
            {
                cell = cells[col / 64];
                opcode = (ushort)((row << 12) | col);
                Signal[] decoded = BlockCellTable[cell]();
                if(decoded.Length == 0) continue;
                decoded[0].Name = name;
                table[opcode] = decoded;
            }
        }
        
        foreach (ushort fixedOpcode in FixedOpcodeTable.Keys)
            table[fixedOpcode] = FixedOpcodeTable[fixedOpcode]();
        
        return table;
    }

    private static readonly Dictionary<string, Func<Signal[]>> BlockCellTable = new()
    {
        ["-"] = () => NONE,
        ["MOV"] = () => TWO_OPERAND(Operation.PASS, true, Width.WORD), ["MOVB"] = () => TWO_OPERAND(Operation.PASS, true, Width.BYTE),
        ["CMP"] = () => TWO_OPERAND(Operation.SUB, false, Width.WORD), ["CMPB"] = () => TWO_OPERAND(Operation.SUB, false, Width.BYTE),
        ["BIT"] = () => TWO_OPERAND(Operation.BIT, false, Width.WORD), ["BITB"] = () => TWO_OPERAND(Operation.BIT, false, Width.BYTE),
        ["BIC"] = () => TWO_OPERAND(Operation.BIC, true, Width.WORD), ["BICB"] = () => TWO_OPERAND(Operation.BIC, true, Width.BYTE),
        ["BIS"] = () => TWO_OPERAND(Operation.BIS, true, Width.WORD), ["BISB"] = () => TWO_OPERAND(Operation.BIS, true, Width.BYTE),
        ["ADD"] = () => TWO_OPERAND(Operation.ADD, true, Width.WORD), ["SUB"] = () => TWO_OPERAND(Operation.SUB, true, Width.WORD),

        ["CLR"] = () => ONE_OPERAND(Operation.CLR, Width.WORD), ["CLRB"] = () => ONE_OPERAND(Operation.CLR, Width.BYTE),
        ["COM"] = () => ONE_OPERAND(Operation.COM, Width.WORD), ["COMB"] = () => ONE_OPERAND(Operation.COM, Width.BYTE),
        ["INC"] = () => ONE_OPERAND(Operation.INC, Width.WORD), ["INCB"] = () => ONE_OPERAND(Operation.INC, Width.BYTE),
        ["DEC"] = () => ONE_OPERAND(Operation.DEC, Width.WORD), ["DECB"] = () => ONE_OPERAND(Operation.DEC, Width.BYTE),
        ["NEG"] = () => ONE_OPERAND(Operation.NEG, Width.WORD), ["NEGB"] = () => ONE_OPERAND(Operation.NEG, Width.BYTE),
        ["ADC"] = () => ONE_OPERAND(Operation.ADC, Width.WORD), ["ADCB"] = () => ONE_OPERAND(Operation.ADC, Width.BYTE),
        ["SBC"] = () => ONE_OPERAND(Operation.SBC, Width.WORD), ["SBCB"] = () => ONE_OPERAND(Operation.SBC, Width.BYTE),
        ["TST"] = () => ONE_OPERAND(Operation.TST, Width.WORD), ["TSTB"] = () => ONE_OPERAND(Operation.TST, Width.BYTE),
        ["ROR"] = () => ONE_OPERAND(Operation.ROR, Width.WORD), ["RORB"] = () => ONE_OPERAND(Operation.ROR, Width.BYTE),
        ["ROL"] = () => ONE_OPERAND(Operation.ROL, Width.WORD), ["ROLB"] = () => ONE_OPERAND(Operation.ROL, Width.BYTE),
        ["ASR"] = () => ONE_OPERAND(Operation.ASR, Width.WORD), ["ASRB"] = () => ONE_OPERAND(Operation.ASR, Width.BYTE),
        ["ASL"] = () => ONE_OPERAND(Operation.ASL, Width.WORD), ["ASLB"] = () => ONE_OPERAND(Operation.ASL, Width.BYTE),
        ["SXT"] = () => ONE_OPERAND(Operation.SXT, Width.WORD), ["SWAB"] = () => ONE_OPERAND(Operation.SWAB, Width.WORD),

        ["BRC"] = BRANCH, ["JMP"] = JUMP,
        ["JSR"] = () => NONE,
        ["MFPI"] = () => NONE, ["MTPI"] = () => NONE,
        ["MFPD"] = () => NONE, ["MTPD"] = () => NONE,
        ["MUL"] = () => NONE, ["DIV"] = () => NONE,
        ["ASH"] = () => NONE, ["ASHC"] = () => NONE,
        ["XOR"] = () => NONE, ["SOB"] = () => NONE,
        ["EMT"] = () => NONE, ["TRAP"] = () => NONE, ["MARK"] = () => NONE,
    };
    
    private static readonly Dictionary<ushort, Func<Signal[]>> FixedOpcodeTable = new()
    {
        [0x0000] = () => STATE_CHANGE(State.HALT, "HALT"), // HALT
        [0x00A0] = () => STATE_CHANGE(State.FETCH, "NOP"), // NOP
    };
    
    private static string AddressingModeName(byte nibble)
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
    private static byte ooxxoo()
        => (byte)((opcode >> 6) & 0x3F);
    private static byte ooooxx()
        => (byte)(opcode & 0x3F);
}