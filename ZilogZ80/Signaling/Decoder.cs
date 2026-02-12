namespace ZilogZ80.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] MainPage = [];
    private readonly Signal[][] MiscPage = [];
    private readonly Signal[][] BitPage = [];
    private readonly Signal[][] IxPage = [];
    private readonly Signal[][] IyPage = [];
    private readonly Signal[][] IxBitPage = [];
    private readonly Signal[][] IyBitPage = [];

    public readonly Signal[] Fetch = Microcode.FETCH;
    private readonly Signal[] Save = Microcode.SAVE;
    
    private Dictionary<byte, Func<bool>>? PrefixTable;
    
    private byte pageIndex;
    private bool dispSwap;
    
    public void Init()
    {
        PrefixTable = new()
            { { 0xED, ED }, { 0xCB, CB }, { 0xDD, DD }, { 0xFD, FD }, };
    }
    
    private bool ED()
    {
        pageIndex = 1;
        dispSwap = false;
        return true;
    }
    private bool CB()
    {
        switch (pageIndex)
        {
            case 0:
                pageIndex += 2;
                return true;
            case 3 or 4:
                pageIndex += 2;
                dispSwap = true;
                return true;
            default:
                return false;
        }
    }
    private bool DD()
    {
        if (pageIndex is not (0 or 3 or 4)) return false;
        pageIndex = 3;
        dispSwap = false;
        return true;
    }
    private bool FD()
    {
        if (pageIndex is not (0 or 3 or 4)) return false;
        pageIndex = 4;
        dispSwap = false;
        return true;
    }

    public Signal[] Decode(byte opcode)
    {
        Signal[] output;
        
        if (PrefixTable!.TryGetValue(opcode, out var PrefixMethod))
            output = PrefixMethod() ? MainPage[opcode] : IndexPage(opcode);
        else
            output = !dispSwap ? IndexPage(opcode) : DispSwap();
        
        return output.Length != 0
            ? output
            : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
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

    private Signal[] DispSwap()
    {
        dispSwap = false;
        return Save;
    }

    public void Clear()
    {
        pageIndex = 0;
        dispSwap = false;
    }
}
