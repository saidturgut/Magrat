namespace Intel8080.Decoding;
using Kernel;

public static class Microcode
{
    public static Signal[][] PageMain(bool dump)
    {
        ZilogZ80.Decoding.Microcode.SetDefault(); 
        return ZilogZ80.Decoding.Microcode.ParseOpcodeTable("8080_PageMain", ZilogZ80.Decoding.Microcode.MainPage, dump);
    }

    public static Signal[] FETCH => ZilogZ80.Decoding.Microcode.FETCH_DEF;
    public static Signal[] INT_1 => [];
}