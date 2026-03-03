namespace Models.Pdp1170.Kernel.Microcodes;

public partial class Microcode
{
    private static ushort opcode;
    private static string name = "";
    private static string cell = "";

    public static readonly string[] LoggerNames = new string[256 * 256];
    
    public static Signal[][] GenerateTable()
    {
        Signal[][] table = new Signal[256 * 256][];
        string[] lines = File.ReadAllLines("Pdp11Matrix.csv");
        for (int row = 0; row < 16; row++)
        {
            string[] cells = lines[row].Split(',');
            for (int col = 0; col < 4096; col++)
            {
                cell = cells[col / 64];
                opcode = (ushort)((row << 12) | col);
                var decoded = BlockCellTable[cell]();
                LoggerNames[opcode] = name;
                table[opcode] = decoded;
            }
        }

        foreach (ushort fixedOpcode in FixedOpcodeTable.Keys)
        {
            opcode = fixedOpcode;
            table[fixedOpcode] = FixedOpcodeTable[fixedOpcode]();
            LoggerNames[fixedOpcode] = name;
        }
        
        //Console.WriteLine(GC.GetTotalMemory(false));
        
        return table;
    }
    
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
    
    public struct Descriptor
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