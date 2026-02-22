namespace Models.Pdp1170.Bus;

public static class Hex
{
    public static void Run(byte[] Memory)
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
}
