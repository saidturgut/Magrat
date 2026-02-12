namespace Mos6502.Signaling.Microcodes;

public partial class Microcode
{
    public static Signal[][] OpcodeRom(bool dump)
    {
        Signal[][] table = new Signal[256][];
        string[] lines = File.ReadAllLines("Opcodes.csv");

        for (int row = 0; row < 16; row++)
        {
            string[] cells = lines[row].Split(',').Select(x => x.Trim()).ToArray();   
            
            for (int col = 0; col < 16; col++)
            {
                string[] cell = cells[col].Split(' ');
                var index = (row << 4) | col;
                table[index] = [..AddressingTable[cell[1]](), ..MnemonicTable[cell[0]]()];
                table[index][0].Name = $"{cell[0]} {DebugAddressingModes[cell[1]]}";
            }
        }
        
        if (dump)
        {
            for (int i = 0; i < 256; i++)
            {
                Console.WriteLine($"OPCODE {i:X2}");
                foreach (Signal signal in table[i]) Console.WriteLine(signal.Cycle);
                Console.WriteLine("-----------------------------");
            }
        
            Environment.Exit(20);
        }
        
        return table;
    }
    
    private static readonly Dictionary<string, string> DebugAddressingModes = new()
    {
        {"-", "ILLEGAL"}, {"IMP", "imp"}, {"IMM", "#$??"}, {"ZP", "$??"}, {"ZPX", "$??,X"}, {"ZPY", "$??,Y"},
        {"ABS", "$????"}, {"ABSX", "$????,X"}, {"ABSY", "$????,Y"}, {"IND", "($????)"}, {"INDX", "($??,X)"}, {"INDY", "($??),Y"},
    };
}