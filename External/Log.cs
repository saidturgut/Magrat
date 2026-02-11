namespace External;

public static class Log
{
    public static string Hex(uint input)         
        => $"0x{System.Convert.ToString(input, 16).ToUpper()}";
}
