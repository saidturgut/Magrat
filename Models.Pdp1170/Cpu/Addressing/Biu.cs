namespace Models.Pdp1170.Cpu.Addressing;
using Bus;

public class Biu
{
    private Unibus Unibus = null!;
    
    private readonly Cache Cache = new ();

    private bool cacheable;
    
    private const byte priority = 4;
    
    public void Init(Unibus unibus)
    {
        Unibus = unibus;
    }

    public void Prepare(uint address)
    {
        cacheable = Unibus.Cacheable(address);
        if(cacheable) Cache.Extract(address);
    }
    
    public bool Ready()
    {
        if (!cacheable) 
            return Unibus.Request(priority);
        
        if (Cache.Hit())
            return true;

        if (!Unibus.Request(priority)) 
            return false;

        Cache.Miss(Unibus);
        
        return true;
    }

    public ushort Read(uint address, Width width)
    {
        if (cacheable) return Cache.Read(width);
        ushort data = width is Width.WORD ? Unibus.ReadWord(address) :  Unibus.ReadByte(address);
        Unibus.Release();
        return data;
    }
    
    public void Write(uint address, ushort value, Width width)
    {
        if (cacheable) Cache.Write(value, width);
        else
        {
            switch (width)
            {
                case Width.WORD: Unibus.WriteWord(address, value); break;
                case Width.BYTE: Unibus.WriteByte(address, (byte)value); break;
            }
            Unibus.Release();
        }
    }
}