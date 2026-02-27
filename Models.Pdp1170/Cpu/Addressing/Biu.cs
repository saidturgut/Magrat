namespace Models.Pdp1170.Cpu.Addressing;
using Bus;

public class Biu
{
    private Unibus Unibus = null!;
    
    private readonly Cache Cache = new ();

    private const byte priority = 4;
    
    public void Init(Unibus unibus)
    {
        Unibus = unibus;
    }
    
    public bool Ready(uint address)
    {
        if (!Unibus.Cacheable(address)) 
            return Unibus.Request(priority);
        
        Cache.Extract(address);
        
        if (Cache.Hit())
            return true;

        if (!Unibus.Request(priority)) 
            return false;

        Cache.Miss(Unibus);
        
        Unibus.Release();
        
        return true;
    }

    public ushort Read(uint address, Width width)
    {
        return Cache.Read(width);
    }

    public void Write(uint address, ushort value, Width width)
    {
        Cache.Write(value, width);
    }
}