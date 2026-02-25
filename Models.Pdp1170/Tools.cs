namespace Models.Pdp1170;

public static class Tools
{
    public static void Dump(byte[] Memory)
    {
        const int bytesPerLine = 16;

        for (var i = 0; i < Memory.Length; i += bytesPerLine)
        {
            Console.Write($"{0 + i:X4}: ");

            for (var j = 0; j < bytesPerLine; j++)
            {
                Console.Write(i + j < Memory.Length ? 
                    $"{Memory[i + j]:X2} " : "   ");
            }

            Console.Write(" |");

            for (var j = 0; j < bytesPerLine && i + j < Memory.Length; j++)
            {
                var b = Memory[i + j];
                Console.Write(b is >= 32 and <= 126 ? (char)b : '.');
            }

            Console.WriteLine("|");
        }
        
        Environment.Exit(08);
    }

    public static string Octal(uint input)         
        => $"0x{Convert.ToString(input, 16).ToUpper()}";
    public static string Hex(uint input)         
        => $"0x{Convert.ToString(input, 16).ToUpper()}";
}