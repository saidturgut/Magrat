namespace External;

// NOT MY WORK
public static class HexDump
{
    public static void Run(byte[] Memory, uint start, uint end)
    {
        const int bytesPerLine = 16;

        for (var i = 0; i < end; i += bytesPerLine)
        {
            Console.Write($"{0 + i:X4}: ");

            for (var j = 0; j < bytesPerLine; j++)
            {
                Console.Write(i + j < end ? 
                    $"{Memory[i + j]:X2} " : "   ");
            }

            Console.Write(" |");

            for (var j = 0; j < bytesPerLine && i + j < end; j++)
            {
                var b = Memory[i + j];
                Console.Write(b is >= 32 and <= 126 ? (char)b : '.');
            }

            Console.WriteLine("|");
        }
        
        Environment.Exit(08);
    }
}
