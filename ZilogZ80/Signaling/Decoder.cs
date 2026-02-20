namespace ZilogZ80.Signaling;
using Kernel.Devices;
using Decoding;
using Kernel;

public class Decoder : IDecoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);
    private readonly Signal[][] MiscPage = Microcode.PageMisc(false);
    private readonly Signal[][] BitPage = Microcode.PageBit(false);
    private readonly Signal[][] IxPage = Microcode.PageIx(false);
    private readonly Signal[][] IyPage = Microcode.PageIy(false);
    private readonly Signal[][] IxBitPage = Microcode.PageIxBit(false);
    private readonly Signal[][] IyBitPage = Microcode.PageIyBit(false);

    private readonly Signal[] FetchSignals = Microcode.FETCH;
    private readonly Signal[] InterruptSignals = Microcode.INT_1;
    
    private readonly Signal[] Prefix = Microcode.PREFIX;
    
    private byte pageIndex;
    private bool skipByte;
    
    public Signal[] Fetch() => FetchSignals;
    public Signal[] Interrupt() => InterruptSignals;
    
    public Signal[] Decode(byte opcode)
    {
        if (skipByte)
        {
            skipByte = false;
            return Prefix;
        }
        var output = IndexPage(opcode);
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }

    public bool Execute(byte state)
    {
        switch ((State)state)
        {
            case State.DEC_ED: pageIndex = 1; skipByte = false; return true;
            case State.DEC_CB: pageIndex = 2; skipByte = false; return true;
            case State.DEC_DD: pageIndex = 3; skipByte = false; return true;
            case State.DEC_FD: pageIndex = 4; skipByte = false; return true;
            case State.DEC_DDCB: pageIndex = 5; skipByte = true; return true;
            case State.DEC_FDCB: pageIndex = 6; skipByte = true; return true;
            default: return false;
        }
    }
    
    private Signal[] IndexPage(byte opcode) => pageIndex switch
    {
        0 => MainPage[opcode],
        1 => MiscPage[opcode],
        2 => BitPage[opcode],
        3 => IxPage[opcode],
        4 => IyPage[opcode],
        5 => IxBitPage[opcode],
        6 => IyBitPage[opcode],
        _ => throw new Exception($"INVALID PAGE \"{pageIndex}\""),
    };
    
    public void Clear()
    {
        pageIndex = 0;
        skipByte = false;
    }
}
