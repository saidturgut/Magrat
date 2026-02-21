namespace Models.x8Bit.Motorola6809.Signaling;
using Engine.Units;
using Decoding;
using Engine;

public class Decoder : IDecoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);
    private readonly Signal[][] SecondPage = Microcode.PageSecond(false);
    private readonly Signal[][] ThirdPage = Microcode.PageThird(false);
    
    private readonly Signal[][] IndexedPage = [];
    private readonly Signal[][] TransferPage = Microcode.PageTfr(false);
    private readonly Signal[][] ExchangePage = Microcode.PageExg(false);

    private readonly Signal[] FetchSignals = Microcode.FETCH;
    private readonly Signal[] InterruptSignals = [];

    private byte pageIndex;
    private bool postByte;
    private byte decoderLatch;
    
    public Signal[] Fetch() => FetchSignals;
    public Signal[] Interrupt() => InterruptSignals;

    public Signal[] Decode(byte opcode)
    {
        Signal[] output;
        if (postByte)
        {
            postByte = false;
            output = PostPage(decoderLatch, opcode);
        }
        else if(PostByteUser(opcode))
        {
            postByte = true;
            decoderLatch = opcode;
            output = Fetch();
        }
        else
        {
            output = IndexPage(opcode);
        }
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }
    
    public bool Execute(byte state)
    {
        switch ((State)state)
        {
            case State.DEC_10: pageIndex = 1; return true;
            case State.DEC_11: pageIndex = 2; return true;
            default: return false;
        }
    }
    
    private Signal[] IndexPage(byte opcode) => pageIndex switch
    {
        0 => MainPage[opcode],
        1 => SecondPage[opcode], 
        2 => ThirdPage[opcode],
        _ => throw new Exception($"INVALID PAGE \"{pageIndex}\""),
    };

    private Signal[] PostPage(byte opcode, byte post) => opcode switch
    {
        0x1E => TransferPage[post],
        0x1F => ExchangePage[post], 
        _ => [..IndexedPage[post], ..IndexPage(opcode)],
    };

    private bool PostByteUser(byte opcode)
        => (byte)(opcode & 0xF) is 0x6 or 0xA or 0xE || opcode is 0x1E or 0x1F;
    
    public void Clear()
    {
        decoderLatch = 0;
        postByte = false;
        pageIndex = 0;
    }
}
