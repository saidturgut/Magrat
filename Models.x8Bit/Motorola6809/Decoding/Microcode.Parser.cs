namespace Models.x8Bit.Motorola6809.Decoding;
using Microsoft.VisualBasic.FileIO;
using Engine;

public partial class Microcode
{
    private static TextFieldParser Parser(string file)
    {
        var parser = new TextFieldParser($"{file}.csv");
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        return parser;
    }
    
    private static Signal[][] ParseOpcodeTable(string file, bool dump)
    {
        var table = new Signal[256][];
        using var parser = Parser(file);
        byte row = 0;
        while (!parser.EndOfData)
        {
            var cells = parser.ReadFields();
            for (byte col = 0; col < 16; col++)
            {
                string[] cell = cells![col].Split('\n');
                byte opcode = (byte)((row << 4) | col);
                table[opcode] = [..ModesPage[cell[1].Trim()](), ..MainPage[cell[0].Trim()]()];
            }
            row++;
        }
        return table;
    }
    
    private static Signal[][] ParseRegisterTable(Func<Pointer, Pointer, Signal[]> byteMethod, Func<Pointer[], Pointer[], Signal[]> wordMethod)
    {
        var table = new Signal[256][];
        using var parser = Parser("6809_PageReg.csv");
        byte row = 0;
        while (!parser.EndOfData)
        {
            var cells = parser.ReadFields();
            for (byte col = 0; col < 16; col++)
            {
                string[] cell = cells![col].Split('\n');
                byte opcode = (byte)((row << 4) | col);
                
                if (cell[0] == "-")
                {
                    table[opcode] = []; continue;
                }

                if (opcode >= 0x88) table[opcode] = byteMethod(RegTable[cell[0]], RegTable[cell[1]]);
                else table[opcode] = wordMethod(PairTable[cell[0]], PairTable[cell[1]]);
            }
            row++;
        }
        return table;
    }

    private static readonly Dictionary<string, Pointer> RegTable = new()
    {
        {"A", Pointer.A}, {"B", Pointer.B}, {"FR", Pointer.FR}, {"DP", Pointer.DP},
    };
    
    private static readonly Dictionary<string, Pointer[]> PairTable = new()
    {
        {"D", [Pointer.A, Pointer.B]}, {"IX", [Pointer.IXL, Pointer.IXH]}, {"IY", [Pointer.IYL, Pointer.IYH]}, 
        {"U", [Pointer.UPL, Pointer.UPH]}, {"S", [Pointer.SPL, Pointer.SPH]}, {"PC", [Pointer.PCL, Pointer.PCH]},
    };
}