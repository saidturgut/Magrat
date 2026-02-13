namespace ZilogZ80.Signaling.Microcodes;
using Microsoft.VisualBasic.FileIO;

public partial class Microcode
{
    private static byte aa_XXa_aaa;
    private static byte aa_XXX_aaa;
    private static byte aa_aaa_XXX;

    public static Signal[][] OpcodeRom(bool dump)
    {
        var table = new Signal[256][];

        using var parser = new TextFieldParser("MainPage.csv");
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        int row = 0;
        while (!parser.EndOfData)
        {
            var cells = parser.ReadFields();

            for (int col = 0; col < 16; col++)
            {
                string[] cell = cells[col].Split('\n');
                var opcode = (row << 4) | col;

                aa_aaa_XXX = (byte)(opcode & 0x7);
                aa_XXa_aaa = (byte)((opcode >> 4) & 0x3);
                aa_XXX_aaa = (byte)((opcode >> 3) & 0x7);

                table[opcode] = MainPage[cell[0].Trim()]();
                table[opcode][0].Name = cell[1];
            }

            row++;
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
}