namespace Models.Pdp1170.Bus;
using Cpu;

public class Unibus
{
    public void Init()
    {
    }

    public bool Request(byte priority)
    {
        return true;
    }
    
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