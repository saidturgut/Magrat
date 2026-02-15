namespace ZilogZ80.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] MainPage = Microcode.PageMain(false);
    private readonly Signal[][] MiscPage = [];
    private readonly Signal[][] BitPage = Microcode.PageBit(false);
    private readonly Signal[][] IxPage = Microcode.PageIx(false);
    private readonly Signal[][] IyPage = [];
    private readonly Signal[][] IxBitPage = Microcode.PageIxBit(false);
    private readonly Signal[][] IyBitPage = [];
    
    public readonly Signal[] Fetch = Microcode.FETCH;
    private readonly Signal[] Prefix = Microcode.PREFIX;
    
    private byte pageIndex;
    
    public Signal[] Decode(byte opcode)
    {
        var output = PrefixCheck(opcode) ? Prefix : IndexPage(opcode);
        return output.Length != 0 ? output : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }

    private bool PrefixCheck(byte opcode)
    {
        switch (opcode)
        {
            case 0xED: pageIndex = 1; return true;
            case 0xCB:
            {
                if (pageIndex is not (0 or 3 or 4)) return false;
                pageIndex += 2; return true;
            }
            case 0xDD:
            {
                if (pageIndex is not (0 or 3 or 4)) return false;
                pageIndex = 3; return true;
            }
            case 0xFD:
            {
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
        pageIndex = 0;
    }
}
