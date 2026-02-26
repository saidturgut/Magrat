namespace Models.Pdp1170.Cpu.Addressing;
using Bus;

public class Biu
{
    private Unibus Unibus = null!;
    
    private readonly Cache Cache = new ();

    public void Init(Unibus unibus)
    {
        Unibus = unibus;
    }
    
    public bool Request(uint address, byte level)
    {
        if (Unibus.Cacheable(address) && Cache.Check(address))
            return true;
        
        if(Unibus.Request(level))
            return true;

        return false;
    }
    
    public byte ReadByte(uint address)
    {
    }
    public ushort ReadWord(uint address)
    {
    }

    public void WriteByte(uint address, byte value)
    {
    }
    public void WriteWord(uint address, ushort value)
    {
    }
}