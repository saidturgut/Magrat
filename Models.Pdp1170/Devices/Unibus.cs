namespace Models.Pdp1170.Devices;

public class Unibus() : Bus(5)
{
    public byte ReadByte(uint address)
    {
        return 0;
    }
    public ushort ReadWord(uint address)
    {
        return 0;
    }

    public void WriteByte(uint address, byte value)
    {
    }
    public void WriteWord(uint address, ushort value)
    {
    }
}
