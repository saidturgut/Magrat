namespace Models.Pdp1170.Devices;

public class Ram
{
    private readonly byte[] Memory = new byte[0x10000];

    public void Dump()
        => Tools.Dump(Memory);
    
    public byte Read(uint address)
        => Memory[address];

    public void Write(uint address, byte data)
        => Memory[address] = data;
}