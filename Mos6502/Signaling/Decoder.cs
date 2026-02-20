namespace Mos6502.Signaling;
using Kernel.Devices;
using Decoding;
using Kernel;


public class Decoder : IDecoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);

    private readonly Signal[] FetchSignals = Microcode.FETCH;
    private readonly Signal[] InterruptSignals = Microcode.BREAK(false);
    
    public Signal[] Fetch() => FetchSignals;
    public Signal[] Interrupt() => InterruptSignals;
    
    public Signal[] Decode(byte opcode)
    {
        var output = MainPage[opcode];
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }

    public bool Execute(byte state)
        => false;

    public void Clear(){}
}
