namespace Models.x8Bit.ZilogZ80.Decoding;
using Microsoft.VisualBasic.FileIO;
using Signaling;
using Models.x8Bit.Engine;

public partial class Microcode
{
    private static byte aa_XXa_aaa;
    private static byte aa_XXX_aaa;
    private static byte aa_aaa_XXX;
    private static Pointer[] addrSrc = [];

    public static Signal[][] ParseOpcodeTable(string file, Dictionary<string, Func<Signal[]>> dictionary, bool dump)
    {
        var table = new Signal[256][];
        using var parser = new TextFieldParser($"{file}.csv");
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        byte row = 0;
        while (!parser.EndOfData)
        {
            var cells = parser.ReadFields();
            for (byte col = 0; col < 16; col++)
            {
                string[] cell = cells![col].Split('\n');
                byte opcode = (byte)((row << 4) | col);
                SetNibbles(opcode);
                table[opcode] = dictionary[cell[0].Trim()]();
                table[opcode][0].Name = cell[1];
            }
            row++;
        }
        if(dump) Dump(table);
        return table;
    }

    private static void Dump(Signal[][] table)
    {
        for (ushort i = 0; i < 256; i++)
        {
            Console.WriteLine($"OPCODE {i:X2}");
            foreach (Signal signal in table[i]) Console.WriteLine(signal.Cycle);
            Console.WriteLine("-----------------------------");
        }
        
        Environment.Exit(20);
    }

    private static void SetNibbles(byte opcode)
    {
        aa_aaa_XXX = (byte)(opcode & 0x7);
        aa_XXa_aaa = (byte)((opcode >> 4) & 0x3);
        aa_XXX_aaa = (byte)((opcode >> 3) & 0x7);
    }
}