namespace Devices;

public class Ram
{
    private readonly byte[] Memory = new byte[0xE000];

    public byte Read(uint address)
        => Memory[address];

    public void Write(uint address, byte data)
        => Memory[address] = data;
}