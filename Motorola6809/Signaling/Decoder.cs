namespace Motorola6809.Signaling;
using Decoding;
using Kernel;

public class Decoder
{
    private readonly Signal[][] MainPage = [];
    private readonly Signal[][] TwoPage = [];
    private readonly Signal[][] ThreePage = [];
    private readonly Signal[][] PostBytes = [];

    public Signal[] Fetch() => Microcode.FETCH;
    public Signal[] Interrupt() => [];

    private byte pageIndex;
    private bool postByte;
    private byte decoderLatch;
    
    public Signal[] Decode(byte opcode)
    {
        Signal[] output;
        if (postByte)
        {
            postByte = false;
            output = [..PostBytes[opcode], ..IndexPage(decoderLatch)];
        }
        else if((byte)(opcode & 0xF) is 0x6 or 0xA or 0xE)
        {
            postByte = true;
            decoderLatch = opcode;
            output = Fetch();
        }
        else
        {
            output = PrefixCheck(opcode) ? Fetch() : IndexPage(opcode);
        }
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }
    
    private bool PrefixCheck(byte opcode)
    {
        switch (opcode)
        {
            case 0x10: pageIndex = 1; return true;
            case 0x11: pageIndex = 2; return true;
            default: return false;
        }
    }
    
    private Signal[] IndexPage(byte opcode) => pageIndex switch
    {
        0 => MainPage[opcode],
        1 => TwoPage[opcode], 
        2 => ThreePage[opcode],
        _ => throw new Exception($"INVALID PAGE \"{pageIndex}\""),
    };
    
    public void Clear()
    {
        decoderLatch = 0;
        postByte = false;
        pageIndex = 0;
    }
}
