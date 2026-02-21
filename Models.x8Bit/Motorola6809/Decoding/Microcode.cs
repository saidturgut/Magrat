namespace Models.x8Bit.Motorola6809.Decoding;
using Engine;

public static partial class Microcode
{
    public static Signal[][] PageMain(bool dump)
    {
        return ParseOpcodeTable("6809_PageMain.csv", dump);
    }
    public static Signal[][] PageSecond(bool dump)
    {
        return ParseOpcodeTable("6809_PageSecond.csv", dump);
    }
    public static Signal[][] PageThird(bool dump)
    {
        return ParseOpcodeTable("6809_PageThird.csv", dump);
    }
    
    public static Signal[][] PageTfr(bool dump)
    {
        return ParseRegisterTable(TRANSFER_BYTE, TRANSFER_WORD);
    }

    public static Signal[][] PageExg(bool dump)
    {
        return ParseRegisterTable(EXCHANGE_BYTE, EXCHANGE_WORD);
    }
}
