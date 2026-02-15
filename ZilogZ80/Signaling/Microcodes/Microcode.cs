namespace ZilogZ80.Signaling.Microcodes;

public partial class Microcode
{
    public static Signal[][] PageMain(bool dump)
    {
        SetDefault(); return ParseOpcodeTable("PageMain", MainPage, dump);
    }
    public static Signal[][] PageMisc(bool dump)
    {
        SetDefault(); return ParseOpcodeTable("PageMisc", MiscPage, dump);
    }
    public static Signal[][] PageBit(bool dump)
    {
        SetDefault(); return ParseOpcodeTable("PageBit", BitPage, dump);
    }
    public static Signal[][] PageIx(bool dump)
    {
        SetIx(); return ParseOpcodeTable("PageIx", MainPage, dump);
    }
    public static Signal[][] PageIy(bool dump)
    {
        SetIy(); return ParseOpcodeTable("PageIy", MainPage, dump);
    }

    public static Signal[][] PageIxBit(bool dump)
    {
        SetIx(); return ParseOpcodeTable("PageIxBit", BitPage, dump);
    }

    public static Signal[][] PageIyBit(bool dump)
    {
        SetIy(); return ParseOpcodeTable("PageIyBit", BitPage, dump);
    }

    private static void SetDefault()
    {
        EncodedRegisters[5] = Pointer.L;
        EncodedRegisters[4] = Pointer.H;
        EncodedPairs[2] = [Pointer.L, Pointer.H];
        addrSrc = HL;
    }
    private static void SetIx()
    {
        EncodedRegisters[5] = Pointer.IXL;
        EncodedRegisters[4] = Pointer.IXH;
        EncodedPairs[2] = [Pointer.IXL, Pointer.IXH];
        addrSrc = WZ;
    }
    private static void SetIy()
    {
        EncodedRegisters[5] = Pointer.IYL;
        EncodedRegisters[4] = Pointer.IYH;
        EncodedPairs[2] = [Pointer.IYL, Pointer.IYH];
        addrSrc = WZ;
    }
}