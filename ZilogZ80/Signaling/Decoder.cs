namespace ZilogZ80.Signaling;
using Microcodes;

public partial class Decoder
{
    private readonly Signal[][] MainPage = Microcode.OpcodeRom(false);
    private readonly Signal[][] MiscPage = [];
    private readonly Signal[][] BitPage = [];
    private readonly Signal[][] IxPage = [];
    private readonly Signal[][] IyPage = [];
    private readonly Signal[][] IxBitPage = [];
    private readonly Signal[][] IyBitPage = [];

    public readonly Signal[] Fetch = Microcode.FETCH;
    public readonly Signal[] Disp = Microcode.DISP;
    
    public Signal[] Decode(byte opcode)
    {
        Signal[] output;

        if (PrefixCheck(opcode))
        {
            output = Fetch;
        }
        else if (dispSwap)
        {
            output = Disp;
            dispSwap = false;
        }
        else
        {
            output = IndexPage(opcode);
        }
        
        return output.Length != 0
            ? output
            : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
    }

    private bool PrefixCheck(byte opcode) => opcode switch
    {
        0xED => ED(), 0xCB => CB(), 0xDD => DD(), 0xFD => FD(), _=>false,
    };
    
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
        dispSwap = false;
    }
}
