namespace Intel8080.Signaling;
using Kernel.Devices;
using Decoding;
using Kernel;

public class Decoder : IDecoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);

    public Signal[] Fetch() => Microcode.FETCH;
    public Signal[] Interrupt() => Microcode.INT_1;
    
    public Signal[] Decode(byte opcode)
    {
        var output = MainPage[opcode];
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }

    public bool Execute(byte state)
        => false;

    public void Clear() { }
}
