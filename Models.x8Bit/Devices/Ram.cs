namespace Models.x8Bit.Devices;
using Engine.Utility;

public class Ram
{
    private readonly byte[] Memory = new byte[0x10000];

    public void Dump(uint start, uint end)
        => HexDump.Run(Memory, start, end);
    
    public byte Read(uint address)
        => Memory[address];

    public void Write(uint address, byte data)
        => Memory[address] = data;
}