namespace ZilogZ80.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);
    private readonly Signal[][] MiscPage = [];
    private readonly Signal[][] BitPage = Microcode.PageBit(false);
    private readonly Signal[][] IxPage = Microcode.PageIx(false);
    private readonly Signal[][] IyPage = Microcode.PageIy(false);
    private readonly Signal[][] IxBitPage = Microcode.PageIxBit(false);
    private readonly Signal[][] IyBitPage = Microcode.PageIyBit(false);
    
    public readonly Signal[] Fetch = Microcode.FETCH;
    private readonly Signal[] Prefix = Microcode.PREFIX;
    
    private byte pageIndex;
    private bool skipByte;
    
    public Signal[] Decode(byte opcode)
    {
        if (skipByte)
        {
            skipByte = false;
            return Prefix;
        }
        var output = PrefixCheck(opcode) ? Fetch : IndexPage(opcode);
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }
    
    private bool PrefixCheck(byte opcode)
    {
        switch (opcode)
        {
            case 0xED:
            {
                skipByte = false;
                pageIndex = 1; return true;
            }
            case 0xCB:
            {
                skipByte = false;
                if (pageIndex is not (0 or 3 or 4)) return false;
                if(pageIndex is 3 or 4) skipByte = true;
                pageIndex += 2; return true;
            }
            case 0xDD:
            {
                skipByte = false;
                if (pageIndex is not (0 or 3 or 4)) return false;
                pageIndex = 3; return true;
            }
            case 0xFD:
            {
                skipByte = false;
                if (pageIndex is not (0 or 3 or 4)) return false;
                pageIndex = 4; return true;
            }
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
        skipByte = false;
        pageIndex = 0;
    }
}
